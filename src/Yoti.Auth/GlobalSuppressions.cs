﻿// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2237:Mark ISerializable types with serializable", Justification = "Serializable not available with NET Standard framework", Scope = "type", Target = "~T:Yoti.Auth.Aml.AmlException")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1032:Implement standard exception constructors", Justification = "Parameterless exception constructor is useless in this instance, we should remove this option", Scope = "type", Target = "~T:Yoti.Auth.Exceptions.AmlException")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1032:Implement standard exception constructors", Justification = "Parameterless exception constructor is useless in this instance, we should remove this option", Scope = "type", Target = "~T:Yoti.Auth.Exceptions.YotiProfileException")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1055:Uri return values should not be strings", Justification = "String is preferable", Scope = "member", Target = "~M:Yoti.Auth.Images.Image.GetBase64URI~System.String")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1305:Specify IFormatProvider", Justification = "FXCop incorrectly identifying Guid.ToString() as having a locale option", Scope = "member", Target = "~M:Yoti.Auth.CryptoEngine.GenerateNonce~System.String")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2237:Mark ISerializable types with serializable", Justification = "[Serializable] not available in 'netstandard' target framework", Scope = "type", Target = "~T:Yoti.Auth.YotiProfileException")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S3925:\"ISerializable\" should be implemented correctly", Justification = "[ISerializable] not available in 'netstandard' target framework", Scope = "type", Target = "~T:Yoti.Auth.Exceptions.AmlException")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S3925:\"ISerializable\" should be implemented correctly", Justification = "[ISerializable] not available in 'netstandard' target framework", Scope = "type", Target = "~T:Yoti.Auth.Exceptions.YotiProfileException")]
