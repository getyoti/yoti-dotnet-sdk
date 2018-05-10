Yoti .NET SDK
=============

Welcome to the Yoti .NET SDK. This repo contains the tools and step by step instructions you need to quickly integrate your .NET back-end with Yoti so that your users can share their identity details with your application in a secure and trusted way.

## Table of Contents

1) [An Architectural view](#an-architectural-view) -
High level overview of integration

1) [Enabling the SDK](#enabling-the-sdk) -
How to install our SDK

1) [Client initialisation](#client-initialisation) -
Description on setting up your SDK

1) [Profile retrieval](#profile-retrieval) -
Description on setting up profile

1) [Handling users](#handling-users) -
Description on handling user log on's

1) [AML Integration](#aml-integration) -
How to integrate with Yoti's AML (Anti Money Laundering) service

1) [Running the examples](#running-the-examples)
How to run the example projects provided

1) [API Coverage](#api-coverage) -
Attributes defined

1) [Support](#support) -
Please feel free to reach out

## An Architectural View

To integrate your application with Yoti, your back-end must expose a GET endpoint that Yoti will use to forward tokens.
The endpoint can be configured in Yoti Dashboard when you create/update your application.

The image below shows how your application back-end and Yoti integrate in the context of a Login flow.
Yoti SDK carries out for you steps 6, 7 ,8 and the profile decryption in step 9.

![alt text](login_flow.png "Login flow")

Yoti also allows you to enable user details verification from your mobile app by means of the Android (TBA) and iOS (TBA) SDKs. In that scenario, your Yoti-enabled mobile app is playing both the role of the browser and the Yoti app. By the way, your back-end doesn't need to handle these cases in a significantly different way. You might just decide to handle the `User-Agent` header in order to provide different responses for web and mobile clients.

## Enabling the SDK

To install the Yoti NuGet package you will need to install NuGet. You can find instructions to do that [here](http://docs.nuget.org/ndocs/guides/install-nuget)

To import the latest Yoti SDK into your project, enter the following command from NuGet Package Manager Console in Visual Studio:

```
Install-Package Yoti
```

For other installation methods, see [nuget.org/packages/Yoti](https://www.nuget.org/packages/Yoti)

## Client Initialisation

The YotiClient is the SDK entry point. To initialise it you need include the following snippet inside your endpoint initialisation section:

```cs
const string sdkId = "your-sdk-id";
var privateKeyStream = System.IO.File.OpenText("path/to/your-application-pem-file.pem");
var yotiClient = new YotiClient(sdkId, privateKeyStream);
```

Where:

* `sdkId` is the sdk identifier generated by Yoti Dashboard when you create (and then publish) your app. _Note this is not your Application Identifier which is needed by your clientside code_
* `path/to/your-application-pem-file.pem` - When you create your app on Yoti Dashboard, your browser generates a .pem file. This is the path to that file.

You'll need to add the `Callback URL` of your site to the integration settings section on your [Yoti Applications page](https://www.yoti.com/dashboard/applications). This is where the user will be redirected to upon successful authentication.

## Profile Retrieval

When your application receives a token via the exposed endpoint (it will be assigned to a query string parameter named `token`), you can easily retrieve the user profile by adding the following to your endpoint handler:

```cs
var activityDetails = yotiClient.GetActivityDetails(token);
```

Or if you are in an asynchronous method:

```cs
var activityDetails = await yotiClient.GetActivityDetailsAsync(token);
```

Before you inspect the user profile, you might want to check whether the user validation was successful.
This is done as follows:

```cs
var activityDetails = yotiClient.GetActivityDetails(token);
if (activityDetails.Outcome == ActivityOutcome.Success)
{
    var profile = activityDetails.UserProfile;
}
else
{
    // handle unhappy path
}
```

## Handling Users

When you retrieve the user profile, you receive a user ID generated by Yoti exclusively for your application.
This means that if the same individual logs into another app, Yoti will assign her/him a different ID.
You can use this ID to verify whether (for your application) the retrieved profile identifies a new or an existing user.
Here is an example of how this works:

```cs
var activityDetails = yotiClient.GetActivityDetails(token);
if (activityDetails.Outcome == ActivityOutcome.Success)
{
    var profile = activityDetails.Profile;
    var user = YourUserSearchFunction(profile.Id);
    if (user != null)
    {
        string userId = profile.Id;
        Image selfie = profile.Selfie.GetImage();
        string selfieURI = profile.Selfie.GetBase64URI();
        string fullName = (string)profile.FullName.GetValue();
        string givenNames = (string)profile.GivenNames.GetValue();
        string familyName = (string)profile.FamilyName.GetValue();
        string mobileNumber = (string)profile.MobileNumber.GetValue();
        string emailAddress = (string)profile.EmailAddress.GetValue();
        DateTime? dateOfBirth = (DateTime?)profile.DateOfBirth.GetValue();
        bool? ageVerified = (bool?)profile.AgeVerified.GetValue();
        string address = (string)profile.Address.GetValue();
        string gender = (string)profile.Gender.GetValue();
        string nationality = (string)profile.Nationality.GetValue();
    }
    else
    {
        // handle registration
    }
}
else
{
    // handle unhappy path
}
```

Where `yourUserSearchFunction` is a piece of logic in your app that is supposed to find a user, given a userId.
No matter if the user is a new or an existing one, Yoti will always provide her/his profile, so you don't necessarily need to store it.

The `profile` object provides a set of attributes corresponding to user attributes. Whether the attributes are present or not depends on the settings you have applied to your app on Yoti Dashboard.

You can retrieve the sources and verifiers for each attribute like this:
```cs
HashSet<string> givenNamesSources = profile.GivenNames.GetSources();
HashSet<string> givenNamesVerifiers = profile.GivenNames.GetVerifiers();
```

## AML Integration

Yoti provides an AML (Anti Money Laundering) check service to allow a deeper KYC process to prevent fraud. This is a chargeable service, so please contact [sdksupport@yoti.com](mailto:sdksupport@yoti.com) for more information.

Yoti will provide a boolean result on the following checks:

* PEP list - Verify against Politically Exposed Persons list
* Fraud list - Verify against  US Social Security Administration Fraud (SSN Fraud) list
* Watch list - Verify against watch lists from the Office of Foreign Assets Control

To use this functionality you must ensure your application is assigned to your Organisation in the Yoti Dashboard - please see [here](https://www.yoti.com/developers/documentation/#1-creating-an-organisation) for further information.

For the AML check you will need to provide the following:

* Data provided by Yoti (please ensure you have selected the Given name(s) and Family name attributes from the Data tab in the Yoti Dashboard)
  * Given name(s)
  * Family name
* Data that must be collected from the user:
  * Country of residence (must be an ISO 3166 3-letter code)
  * Social Security Number (US citizens only)
  * Postcode/Zip code (US citizens only)

### Consent

Performing an AML check on a person *requires* their consent.
**You must ensure you have user consent *before* using this service.**

### Code Example

Given a YotiClient initialised with your SDK ID and KeyPair (see [Client Initialisation](#client-initialisation)) performing an AML check is a straightforward case of providing basic profile data.

```cs
AmlAddress amlAddress = new AmlAddress(
   country: "GBR");

AmlProfile amlProfile = new AmlProfile(
    givenNames: "Edward Richard George",
    familyName: "Heath",
    amlAddress: amlAddress);

AmlResult amlResult = yotiClient.PerformAmlCheck(amlProfile);

bool onPepList = amlResult.IsOnPepList();
bool onFraudList = amlResult.IsOnFraudList();
bool onWatchList = amlResult.IsOnWatchList();
```

## Running the Examples

### .NET 4.5 (Windows only)

1) Open the Example.sln solution in Visual Studio, found in the [/src/Examples](/src/Examples) folder
1) Rename the [secrets.config.example](src/Examples/45Example/secrets.config.example) file to `secrets.config`
1) Fill in the environment variables in this file with the ones specific to your application (mentioned in the [Client initialisation](#client-initialisation) section)
1) From the Yoti Dashboard, set the Callback URL of your application to be `localhost:57045/account/connect`. You'll need to change it from `https` to `http` once the webpage loads, as the Dashboard doesn't allow `http` URLs
1) Run the [45Example.csproj](src/Examples/45Example/45Example.csproj) with your browser of choice
1) The page should open automatically with URL `http://localhost:57045/Account/Login`

### .NET Core

#### Requirements
1) Install Bower with `npm install -g bower`
1) The .NET SDK for your operating system from step no.1 ([Windows](https://www.microsoft.com/net/learn/get-started/windows) | [Linux](https://www.microsoft.com/net/learn/get-started/linux/rhel) | [MacOS](https://www.microsoft.com/net/learn/get-started/macos))

#### Instructions
1) Navigate to the [/src/Examples/CoreExample](src/Examples/CoreExample) folder
1) Rename the [.env.example](src/Examples/CoreExample/.env.example) file to `.env`
1) Fill in the environment variables in this file with the ones specific to your application (mentioned in the [Client initialisation](#client-initialisation) section)
1) From the Yoti Dashboard, set the Callback URL of your application to be `localhost:53647/account/connect`. You'll need to change it from `https` to `http` once the webpage loads, as the Dashboard doesn't allow `http` URLs
1) Restore the static files needed with `bower install`
1) Enter `dotnet run` into the terminal 
1) Navigate to the page specified in the terminal window, which should be `http://localhost:53647`

## API Coverage

* Activity Details
  * [X] Profile
    * [X] User ID `Id`
    * [X] Selfie `Selfie`
    * [X] Selfie URI `Selfie.GetBase64URI()`
    * [X] Given Names `GivenNames`
    * [X] Family Name `FamilyName`
    * [X] Full Name `FullName`
    * [X] Mobile Number `MobileNumber`
    * [X] Email Address `EmailAddress`
    * [X] Age / Date of Birth `DateOfBirth`
    * [X] Age / Age Verified `AgeVerified`
    * [X] Postal Address `Address`
    * [X] Gender `Gender`
    * [X] Nationality `Nationality`
    
## Support

For any questions or support please email [sdksupport@yoti.com](mailto:sdksupport@yoti.com).
Please provide the following to get you up and working as quickly as possible:

* Computer type
* OS version
* Version of .NET being used
* Screenshot

Once we have answered your question we may contact you again to discuss Yoti products and services. If you’d prefer us not to do this, please let us know when you e-mail.

For further documentation, see [https://www.yoti.com/developers/documentation/?csharp](www.yoti.com/developers/documentation/?csharp)
