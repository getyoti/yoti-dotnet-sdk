Yoti .NET SDK Profile Service
=============================

1) [An Architectural View](#an-architectural-view) -
High level overview of integration

1) [Profile Retrieval](#profile-retrieval) -
Description on setting up profile

1) [Handling Users](#handling-users) -
Description on handling user details

1) [Running the Examples](#running-the-examples)
How to run the example projects provided

1) [API Coverage](#api-coverage) -
Attributes defined

## An Architectural View

To integrate your application with Yoti, your back-end must expose a GET endpoint that Yoti will use to forward tokens.
The endpoint can be configured in Yoti Hub when you create/update your application.

The image below shows how your application back-end and Yoti integrate in the context of a Login flow.
Yoti SDK carries out for you steps 6, 7 ,8 and the profile decryption in step 9.

![alt text](login_flow.png "Login flow")

Yoti also allows you to enable user details verification from your mobile app by means of the Android (TBA) and iOS (TBA) SDKs. In that scenario, your Yoti-enabled mobile app is playing both the role of the browser and the Yoti app. By the way, your back-end doesn't need to handle these cases in a significantly different way. You might just decide to handle the `User-Agent` header in order to provide different responses for web and mobile clients.

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
ActivityDetails activityDetails = yotiClient.GetActivityDetails(oneTimeUseToken);

YotiProfile profile = activityDetails.Profile;
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
    List<Yoti.Auth.Images.Image> documentImages = profile.DocumentImages?.GetValue();
    bool? isAgedOver18 = profile.FindAgeOverVerification(18)?.Result();
    bool? isAgedUnder55 = profile.FindAgeUnderVerification(55)?.Result();
}
else
{
    // handle registration
}
```

Where `yourUserSearchFunction` is a piece of logic in your app that is supposed to find a user, given a RememberMeId or ParentRememberMeId.
No matter if the user is a new or an existing one, Yoti will always provide her/his profile, so you don't necessarily need to store it.

The `profile` object provides a set of attributes corresponding to user attributes. Whether the attributes are present or not depends on the settings you have applied to your app on Yoti Hub.

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
Anchor givenNamesFirstSource = profile.GivenNames.GetSources().First();

AnchorType anchorType = givenNamesFirstSource.GetAnchorType();
List<X509Certificate2> originServerCerts = givenNamesFirstSource.GetOriginServerCerts();
byte[] signature = givenNamesFirstSource.GetSignature();
DateTime signedTimeStamp = givenNamesFirstSource.GetSignedTimeStamp().GetTimestamp();
string subType = givenNamesFirstSource.GetSubType();
string value = givenNamesFirstSource.GetValue();
```

## Running the Examples

Follow the below links for instructions on how to run the example projects:
1) [.NET 4.7 example](../src/Examples/Profile/47Example/README.md) (Windows only)
1) [.NET Core example](../src/Examples/Profile/CoreExample/README.md)

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
    * [X] Document Details `DocumentDetails`
    * [X] Document Images `DocumentImages`
    * [X] Age Verifications `AgeVerifications`
  * [X] ApplicationProfile `ApplicationProfile`
    * [X] Name `Name`
    * [X] URL `URL`
    * [X] Logo `Logo`
    * [X] Receipt Background Color `ReceiptBackgroundColor`