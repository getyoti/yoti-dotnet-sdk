# Change Log
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/)
and this project adheres to [Semantic Versioning](http://semver.org/).

## 2.6.0 - 2018-05-16
### Added
- `YotiProfile`, which contains `YotiAttribute` instances.  'YotiAttribute' allows you to get its sources and verifiers by means of the 'GetSources' and 'GetVerifiers' methods

### Changed
- On new YotiProfile:
  - `IsAgeVerified` is now `AgeVerified`
  - Base64URI is now GetBase64URI, used like: `profile.Selfie.GetBase64URI()`
- If Address is null, it is taken from StructuredPostalAddress.FormattedAddress (if present)

## 2.5.0 - 2018-04-16
### Added
- `structured_postal_address` attributes

## 2.4.0 - 2018-03-22
### Added
- 'IsAgeVerified` helper

## 2.3.0 - 2018-03-13
### Added
- AML check functionality

## 2.2.1 - 2018-02-21
### Changed
- Default HTTP protocol to be TLS1.2 - ensuring compatibility with .NET 4.5

## 2.2.0 - 2017-10-27
### Added
- 'Base64URI' helper to Yoti Selfie
- "Download Image" option to Example project

## 2.1.2 - 2017-10-17
### Changed
- License to be MIT

## 2.1.1 - 2017-10-13
### Changed
- HTTP headers sent in requests to now include `X-Yoti-SDK`

## 2.1.0 - 2017-10-03
### Added
- New HTTP header sent in requests: `X-SDK`

## 2.0.0 - 2017-09-20
- Initial public release after package rename from `Yoti.Auth` to `Yoti`