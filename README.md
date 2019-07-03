Yoti .NET SDK
=============

[![Build Status](https://travis-ci.com/getyoti/yoti-dotnet-sdk.svg?branch=master)](https://travis-ci.com/getyoti/yoti-dotnet-sdk)

Welcome to the Yoti .NET SDK. This repo contains the tools and step by step instructions you need to quickly integrate your .NET back-end with Yoti so that your users can share their identity details with your application in a secure and trusted way.

## Table of Contents

1) [An Architectural View](#an-architectural-view) -
High level overview of integration

1) [Enabling the SDK](#enabling-the-sdk) -
How to install our SDK

1) [Client Initialisation](#client-initialisation) -
Description on setting up your SDK

1) [Profile Retrieval](#profile-retrieval) -
Description on setting up profile

1) [Handling Users](#handling-users) -
Description on handling user log on's

1) [AML Integration](#aml-integration) -
How to integrate with Yoti's AML (Anti Money Laundering) service

1) [Running the Profile Examples](#running-the-profile-examples)
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

You'll need to add the correct callback URL to one of the scenarios of your application from the [Yoti Applications page](https://www.yoti.com/dashboard/applications). You also need to make sure that the value of the `data-yoti-scenario-id` attribute in the Yoti button matches the id in the selected scenario.

## Profile Retrieval

When your application receives a **one time use** token via the exposed endpoint (it will be assigned to a query string parameter named `token`), you can easily retrieve the user profile by adding the following to your endpoint handler:

```cs
var activityDetails = yotiClient.GetActivityDetails(oneTimeUseToken);
```

Or if you are in an asynchronous method:

```cs
var activityDetails = await yotiClient.GetActivityDetailsAsync(oneTimeUseToken);
```

## Handling Users
The ActivityDetails includes a unique identifier for the user, named RememberMeId.  You can use this to identify users returning to your application, but if the user completes a share with a different application, Yoti will assign a different RememberMeId.
Applications registered to an organisation may also utilise an additional unique identifier named ParentRememberMeId.  Use this to identify users returning to your organisation.  It is consistent for a given user in all applications registered to a single organisation.

Here is an example of how this works:

```cs
var activityDetails = yotiClient.GetActivityDetails(oneTimeUseToken);

var profile = activityDetails.Profile;
var user = YourUserSearchFunction(activityDetails.RememberMeId); //or use ParentRememberMeId
if (user != null)
{
    Image selfie = profile.Selfie?.GetValue();
    string selfieURI = profile.Selfie?.GetValue().GetBase64URI();
    string fullName = profile.FullName?.GetValue();
    string givenNames = profile.GivenNames?.GetValue();
    string familyName = profile.FamilyName?.GetValue();
    string mobileNumber = profile.MobileNumber?.GetValue();
    string emailAddress = profile.EmailAddress?.GetValue();
    DateTime? dateOfBirth = profile.DateOfBirth?.GetValue();       
    string address = profile.Address?.GetValue();
    Dictionary<string, Newtonsoft.Json.Linq.JToken> structuredPostalAddress = profile.StructuredPostalAddress?.GetValue();
    string gender = profile.Gender?.GetValue();
    string nationality = profile.Nationality?.GetValue();
    Yoti.Auth.Document.DocumentDetails documentDetails = profile.DocumentDetails?.GetValue();
    bool? isAgedOver18;
    AgeVerification over18Verification = profile.FindAgeOverVerification(18);    
    isAgedOver18 = over18Verification?.Result();
}
else
{
    // handle registration
}
```

Where `yourUserSearchFunction` is a piece of logic in your app that is supposed to find a user, given a RememberMeId or ParentRememberMeId.
No matter if the user is a new or an existing one, Yoti will always provide her/his profile, so you don't necessarily need to store it.

The `profile` object provides a set of attributes corresponding to user attributes. Whether the attributes are present or not depends on the settings you have applied to your app on Yoti Dashboard.

You can retrieve the anchors, sources and verifiers for each attribute as follows:
```cs
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Yoti.Auth.Anchors;

List<Anchor> givenNamesAnchors = profile.GivenNames.GetAnchors();
List<Anchor> givenNamesSources = profile.GivenNames.GetSources();
List<Anchor> givenNamesVerifiers = profile.GivenNames.GetVerifiers();
```
You can also retrieve further properties from these respective anchors in the following way:
```cs
Anchor givenNamesFirstAnchor = profile.GivenNames.GetSources().First();

AnchorType anchorType = givenNamesFirstAnchor.GetAnchorType();
List<X509Certificate2> originServerCerts = givenNamesFirstAnchor.GetOriginServerCerts();
byte[] signature = givenNamesFirstAnchor.GetSignature();
DateTime signedTimeStamp = givenNamesFirstAnchor.GetSignedTimeStamp().GetTimestamp();
string subType = givenNamesFirstAnchor.GetSubType();
string value = givenNamesFirstAnchor.GetValue();
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

### AML Example

#### 1) Setup
1) Navigate to the [src/Examples/Aml/AmlExample](src/Examples/Aml/AmlExample) folder
1) Rename the [.env.example](src/Examples/Aml/AmlExample/.env.example) file to `.env`
1) Fill in the environment variables in this file with the ones specific to your application (mentioned in the [Client initialisation](#client-initialisation) section)

#### 2a) With Visual Studio
1) Right click on "AmlExample" in the Solution Explorer and select "Set as StartUp Project"
1) Run the project

#### 2b) From command line
1) (From the [src/Examples/Aml/AmlExample](src/Examples/Aml/AmlExample) folder)  enter `dotnet run`

## Running the Profile Examples

### .NET 4.5 (Windows only)

1) Open the Yoti.Auth.sln solution in Visual Studio, found in the [/src](/src) folder
1) Rename the [secrets.config.example](src/Examples/Profile/45Example/secrets.config.example) file to `secrets.config`
1) Fill in the environment variables in this file with the ones specific to your application (mentioned in the [Client initialisation](#client-initialisation) section)
1) From the Yoti Dashboard, set the application domain to `localhost:44321` and the scenario callback URL to `/account/connect`
1) Right click on "45Example" in the Solution Explorer and select "Set as StartUp Project"
1) Run the project
1) The web page should open automatically with URL `https://localhost:44321`

### .NET Core

#### 1) Setup
1) Navigate to the [src/Examples/Profile/CoreExample](src/Examples/Profile/CoreExample) folder
1) Rename the [.env.example](src/Examples/Profile/CoreExample/.env.example) file to `.env`
1) Fill in the environment variables in this file with the ones specific to your application (mentioned in the [Client initialisation](#client-initialisation) section)

#### 2a) With Docker
1) From the Yoti Dashboard, set the application domain to `localhost:44380` and the scenario callback URL to `/account/connect`
1) `docker-compose build --no-cache`
1) `docker-compose up`
1) Navigate to <https://localhost:44380>

>If you encounter a "permission denied" error when trying to access the mounted .pem file, try disabling and reenabling your shared drive in Docker settings.

#### 2b) With .NET Core installed locally
1) From the Yoti Dashboard, set the application domain to `localhost:44344` and the scenario callback URL to `/account/connect`
1) Download the .NET SDK for your operating system from step no.1 on ([Windows](https://www.microsoft.com/net/learn/get-started/windows) | [Linux](https://www.microsoft.com/net/learn/get-started/linux/rhel) | [MacOS](https://www.microsoft.com/net/learn/get-started/macos))
1) Enter `dotnet run` into the terminal 
1) Navigate to the page specified in the terminal window, which should be <https://localhost:44344>


## API Coverage

* Activity Details
  * [X] RememberMeId `RememberMeId`
  * [X] ParentRememberMeId `ParentRememberMeId`
  * [X] Timestamp `Timestamp`
  * [X] Receipt ID `ReceiptId`
  * [X] Profile `Profile`
    * [X] Selfie `Selfie`
    * [X] Selfie URI `Selfie.GetBase64URI()`
    * [X] Given Names `GivenNames`
    * [X] Family Name `FamilyName`
    * [X] Full Name `FullName`
    * [X] Mobile Number `MobileNumber`
    * [X] Email Address `EmailAddress`
    * [X] Age / Date of Birth `DateOfBirth`
    * [X] Postal Address `Address`
    * [X] Structured Postal Address `StructuredPostalAddress`
    * [X] Gender `Gender`
    * [X] Nationality `Nationality`
    * [X] Age Verifications `AgeVerifications`
  * [X] ApplicationProfile `ApplicationProfile`
    * [X] Name `Name`
    * [X] URL `URL`
    * [X] Logo `Logo`
    * [X] Receipt Background Color `ReceiptBackgroundColor`
    
## Support

For any questions or support please email [sdksupport@yoti.com](mailto:sdksupport@yoti.com).
Please provide the following to get you up and working as quickly as possible:

* Computer type
* OS version
* Version of .NET being used
* Screenshot

Once we have answered your question we may contact you again to discuss Yoti products and services. If you’d prefer us not to do this, please let us know when you e-mail.

For further documentation, see <https://www.yoti.com/developers/documentation/?csharp>
