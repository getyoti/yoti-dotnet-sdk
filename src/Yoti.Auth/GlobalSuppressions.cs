﻿// This file is used by Code Analysis to maintain SuppressMessage attributes that are applied to this
// project. Project-level suppressions either have no target or are given a specific target and
// scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2237:Mark ISerializable types with serializable", Justification = "Serializable not available with NET Standard framework", Scope = "type", Target = "~T:Yoti.Auth.Exceptions.AmlException")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2237:Mark ISerializable types with serializable", Justification = "Serializable not available with NET Standard framework", Scope = "type", Target = "~T:Yoti.Auth.Exceptions.YotiProfileException")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2237:Mark ISerializable types with serializable", Justification = "Serializable not available with NET Standard framework", Scope = "type", Target = "~T:Yoti.Auth.Exceptions.DynamicShareException")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2237:Mark ISerializable types with serializable", Justification = "Serializable not available with NET Standard framework", Scope = "type", Target = "~T:Yoti.Auth.Exceptions.DocScanException")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2237:Mark ISerializable types with serializable", Justification = "Serializable not available with NET Standard framework", Scope = "type", Target = "~T:Yoti.Auth.Exceptions.YotiException")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1032:Implement standard exception constructors", Justification = "Parameterless exception constructor is useless in this instance, we should remove this option", Scope = "type", Target = "~T:Yoti.Auth.Exceptions.AmlException")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1032:Implement standard exception constructors", Justification = "Parameterless exception constructor is useless in this instance, we should remove this option", Scope = "type", Target = "~T:Yoti.Auth.Exceptions.YotiProfileException")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1032:Implement standard exception constructors", Justification = "Parameterless exception constructor is useless in this instance, we should remove this option", Scope = "type", Target = "~T:Yoti.Auth.Exceptions.DynamicShareException")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1305:Specify IFormatProvider", Justification = "FXCop incorrectly identifying Guid.ToString() as having a locale option", Scope = "member", Target = "~M:Yoti.Auth.CryptoEngine.GenerateNonce~System.String")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S3925:\"ISerializable\" should be implemented correctly", Justification = "[ISerializable] not available in 'netstandard' target framework", Scope = "type", Target = "~T:Yoti.Auth.Exceptions.AmlException")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S3925:\"ISerializable\" should be implemented correctly", Justification = "[ISerializable] not available in 'netstandard' target framework", Scope = "type", Target = "~T:Yoti.Auth.Exceptions.YotiProfileException")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S3925:\"ISerializable\" should be implemented correctly", Justification = "[ISerializable] not available in 'netstandard' target framework", Scope = "type", Target = "~T:Yoti.Auth.Exceptions.YotiException")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S3925:\"ISerializable\" should be implemented correctly", Justification = "[ISerializable] not available in 'netstandard' target framework", Scope = "type", Target = "~T:Yoti.Auth.Exceptions.DynamicShareException")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S3925:\"ISerializable\" should be implemented correctly", Justification = "[ISerializable] not available in 'netstandard' target framework", Scope = "type", Target = "~T:Yoti.Auth.Exceptions.ExtraDataException")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S3925:\"ISerializable\" should be implemented correctly", Justification = "[ISerializable] not available in 'netstandard' target framework", Scope = "type", Target = "~T:Yoti.Auth.Exceptions.DocScanException")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "We don't want any exception to stop the remaining attributes from being parsed, so we need to catch the general exception", Scope = "member", Target = "~M:Yoti.Auth.Attribute.AttributeConverter.ConvertToBaseAttributes(Yoti.Auth.ProtoBuf.Attribute.AttributeList)~System.Collections.Generic.Dictionary{System.String,Yoti.Auth.Attribute.BaseAttribute}")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "CA1724:The type name Extension conflicts in whole or in part with the namespace name 'Org.BouncyCastle.X509.Extension'. Change either name to eliminate the conflict.", Justification = "Since this is a class library, clients can fully qualify the name to avoid this ambiguity", Scope = "type", Target = "~T:Yoti.Auth.ShareUrl.Extensions.Extension`1")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0052:Remove unread private members", Justification = "used when creating the DynamicPolicy JSON", Scope = "member", Target = "~F:Yoti.Auth.ShareUrl.Policy.DynamicPolicy._isWantedRememberMeIdOptional")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2237:Mark ISerializable types with serializable", Justification = "[Serializable] not available in 'netstandard' target framework", Scope = "type", Target = "~T:Yoti.Auth.Exceptions.ExtraDataException")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0066:Convert switch statement to expression", Justification = "Other switch cases can all be represented more concisely by the default case", Scope = "member", Target = "~M:Yoti.Auth.Web.Response.CreateYotiExceptionFromStatusCode``1(System.Net.Http.HttpResponseMessage)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1055:Uri return values should not be strings", Justification = "We do not want to cause a breaking change by changing the return type in a minor", Scope = "member", Target = "~M:Yoti.Auth.MediaValue.GetBase64URI~System.String")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1054:Uri parameters should not be strings", Justification = "This is a Url which is being passed as a string in a JSON payload", Scope = "member", Target = "~M:Yoti.Auth.DocScan.Session.Create.SdkConfig.#ctor(System.String,System.String,System.String,System.String,System.String,System.String,System.String,System.String,System.String)")]
[assembly: SuppressMessage("Design", "CA1054:Uri parameters should not be strings", Justification = "This is a string for a JSON payload", Scope = "member", Target = "~M:Yoti.Auth.DocScan.Session.Create.SdkConfigBuilder.WithErrorUrl(System.String)~Yoti.Auth.DocScan.Session.Create.SdkConfigBuilder")]
[assembly: SuppressMessage("Design", "CA1054:Uri parameters should not be strings", Justification = "This is a string for a JSON payload", Scope = "member", Target = "~M:Yoti.Auth.DocScan.Session.Create.SdkConfigBuilder.WithSuccessUrl(System.String)~Yoti.Auth.DocScan.Session.Create.SdkConfigBuilder")]
[assembly: SuppressMessage("Design", "CA1054:Uri parameters should not be strings", Justification = "This is a string for a JSON payload", Scope = "member", Target = "~M:Yoti.Auth.DocScan.Session.Create.SdkConfigBuilder.WithPrivacyPolicyUrl(System.String)~Yoti.Auth.DocScan.Session.Create.SdkConfigBuilder")]
[assembly: SuppressMessage("Design", "CA1056:Uri properties should not be strings", Justification = "This is a string for a JSON payload", Scope = "member", Target = "~P:Yoti.Auth.DocScan.Session.Create.SdkConfig.SuccessUrl")]
[assembly: SuppressMessage("Design", "CA1056:Uri properties should not be strings", Justification = "This is a string for a JSON payload", Scope = "member", Target = "~P:Yoti.Auth.DocScan.Session.Create.SdkConfig.ErrorUrl")]
[assembly: SuppressMessage("Design", "CA1056:Uri properties should not be strings", Justification = "This is a string for a JSON payload", Scope = "member", Target = "~P:Yoti.Auth.DocScan.Session.Create.SdkConfig.PrivacyPolicyUrl")]
[assembly: SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Is a builder - can not be static in future if we add methods to it", Scope = "member", Target = "~M:Yoti.Auth.DocScan.Session.Create.Check.RequestedDocumentAuthenticityCheckBuilder.Build~Yoti.Auth.DocScan.Session.Create.Check.RequestedDocumentAuthenticityCheck")]
[assembly: SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Is a builder - can not be static in future if we add methods to it", Scope = "member", Target = "~M:Yoti.Auth.DocScan.Session.Create.Check.RequestedIdDocumentComparisonCheckBuilder.Build~Yoti.Auth.DocScan.Session.Create.Check.RequestedIdDocumentComparisonCheck")]
[assembly: SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Is a builder - can not be static in future if we add methods to it", Scope = "member", Target = "~M:Yoti.Auth.DocScan.Session.Create.Objectives.ProofOfAddressObjectiveBuilder.Build~Yoti.Auth.DocScan.Session.Create.Objectives.ProofOfAddressObjective")]
[assembly: SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Is a builder - can not be static in future if we add methods to it", Scope = "member", Target = "~M:Yoti.Auth.DocScan.Session.Create.Objectives.ProofOfAddressObjectiveBuilder.Build~Yoti.Auth.DocScan.Session.Create.Objectives.ProofOfAddressObjective")]
[assembly: SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Is a builder - can not be static in future if we add methods to it", Scope = "member", Target = "~M:Yoti.Auth.DocScan.Session.Create.Check.RequestedThirdPartyIdentityCheckBuilder.Build~Yoti.Auth.DocScan.Session.Create.Check.RequestedThirdPartyIdentityCheck")]
[assembly: SuppressMessage("Style", "IDE0063:Use simple 'using' statement", Justification = "This is a preview feature of C#, so can't be compiled with some versions", Scope = "member", Target = "~M:Yoti.Auth.DocScan.DocScanService.CreateSession(System.String,Org.BouncyCastle.Crypto.AsymmetricCipherKeyPair,Yoti.Auth.DocScan.Session.Create.SessionSpecification)~System.Threading.Tasks.Task{Yoti.Auth.DocScan.Session.Create.CreateSessionResult}")]
[assembly: SuppressMessage("Style", "IDE0063:Use simple 'using' statement", Justification = "This is a preview feature of C#, so can't be compiled with some versions", Scope = "member", Target = "~M:Yoti.Auth.DocScan.DocScanService.GetSession(System.String,Org.BouncyCastle.Crypto.AsymmetricCipherKeyPair,System.String)~System.Threading.Tasks.Task{Yoti.Auth.DocScan.Session.Retrieve.GetSessionResult}")]
[assembly: SuppressMessage("Style", "IDE0063:Use simple 'using' statement", Justification = "This is a preview feature of C#, so can't be compiled with some versions", Scope = "member", Target = "~M:Yoti.Auth.DocScan.DocScanService.GetMediaContent(System.String,Org.BouncyCastle.Crypto.AsymmetricCipherKeyPair,System.String,System.String)~System.Threading.Tasks.Task{Yoti.Auth.MediaValue}")]
[assembly: SuppressMessage("Style", "IDE0063:Use simple 'using' statement", Justification = "This is a preview feature of C#, so can't be compiled with some versions", Scope = "member", Target = "~M:Yoti.Auth.YotiClientEngine.GetActivityDetailsAsync(System.String,System.String,Org.BouncyCastle.Crypto.AsymmetricCipherKeyPair,System.Uri)~System.Threading.Tasks.Task{Yoti.Auth.ActivityDetails}")]
[assembly: SuppressMessage("Style", "IDE0063:Use simple 'using' statement", Justification = "This is a preview feature of C#, so can't be compiled with some versions", Scope = "member", Target = "~M:Yoti.Auth.ShareUrl.DynamicSharingService.CreateShareURL(System.Net.Http.HttpClient,System.Uri,System.String,Org.BouncyCastle.Crypto.AsymmetricCipherKeyPair,Yoti.Auth.ShareUrl.DynamicScenario)~System.Threading.Tasks.Task{Yoti.Auth.ShareUrl.ShareUrlResult}")]
[assembly: SuppressMessage("Style", "IDE0063:Use simple 'using' statement", Justification = "This is a preview feature of C#, so can't be compiled with some versions", Scope = "member", Target = "~M:Yoti.Auth.Anchors.AnchorCertificateParser.GetListOfStringsFromExtension(System.Security.Cryptography.X509Certificates.X509Certificate2,System.String)~System.Collections.Generic.List{System.String}")]
[assembly: SuppressMessage("Style", "IDE0063:Use simple 'using' statement", Justification = "This is a preview feature of C#, so can't be compiled with some versions", Scope = "member", Target = "~M:Yoti.Auth.Aml.RemoteAmlService.PerformCheck(System.Net.Http.HttpClient,Org.BouncyCastle.Crypto.AsymmetricCipherKeyPair,System.Uri,System.String,System.Byte[])~System.Threading.Tasks.Task{Yoti.Auth.Aml.AmlResult}")]
[assembly: SuppressMessage("Style", "IDE0063:Use simple 'using' statement", Justification = "This is a preview feature of C#, so can't be compiled with some versions", Scope = "member", Target = "~M:Yoti.Auth.DocScan.DocScanService.DeleteSession(System.String,Org.BouncyCastle.Crypto.AsymmetricCipherKeyPair,System.String)")]
[assembly: SuppressMessage("Style", "IDE0063:Use simple 'using' statement", Justification = "This is a preview feature of C#, so can't be compiled with some versions", Scope = "member", Target = "~M:Yoti.Auth.DocScan.DocScanService.DeleteMediaContent(System.String,Org.BouncyCastle.Crypto.AsymmetricCipherKeyPair,System.String,System.String)")]
[assembly: SuppressMessage("Style", "IDE0063:Use simple 'using' statement", Justification = "This is a preview feature of C#, so can't be compiled with some versions", Scope = "member", Target = "~M:Yoti.Auth.DocScan.DocScanService.GetSupportedDocuments(System.String,Org.BouncyCastle.Crypto.AsymmetricCipherKeyPair)~System.Threading.Tasks.Task{Yoti.Auth.DocScan.Support.SupportedDocumentsResponse}")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "Is validated in Validation.CollectionNotEmpty", Scope = "member", Target = "~M:Yoti.Auth.DocScan.Session.Create.Filter.CountryRestriction.#ctor(System.String,System.Collections.Generic.List{System.String})")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "Is validated in Validation.CollectionNotEmpty", Scope = "member", Target = "~M:Yoti.Auth.DocScan.Session.Create.Filter.TypeRestriction.#ctor(System.String,System.Collections.Generic.List{System.String})")]
[assembly: SuppressMessage("Style", "IDE0090:Use 'new(...)'", Justification = "This is a preview feature of C#, so can't be compiled with some versions", Scope = "member", Target = "~F:Yoti.Auth.DocScan.Session.Create.NotificationConfigBuilder._topics")]
[assembly: SuppressMessage("Style", "IDE0090:Use 'new(...)'", Justification = "This is a preview feature of C#, so can't be compiled with some versions", Scope = "member", Target = "~M:Yoti.Auth.DocScan.Session.Create.Check.RequestedThirdPartyIdentityCheckBuilder.Build~Yoti.Auth.DocScan.Session.Create.Check.RequestedThirdPartyIdentityCheck")]
[assembly: SuppressMessage("Style", "IDE0090:Use 'new(...)'", Justification = "This is a preview feature of C#, so can't be compiled with some versions", Scope = "member", Target = "~M:Yoti.Auth.DocScan.Session.Create.Check.RequestedLivenessCheckBuilder.Build~Yoti.Auth.DocScan.Session.Create.Check.RequestedLivenessCheck")]
[assembly: SuppressMessage("Style", "IDE0090:Use 'new(...)'", Justification = "This is a preview feature of C#, so can't be compiled with some versions", Scope = "member", Target = "~M:Yoti.Auth.DocScan.Session.Create.Check.RequestedFaceMatchCheckBuilder.Build~Yoti.Auth.DocScan.Session.Create.Check.RequestedFaceMatchCheck")]