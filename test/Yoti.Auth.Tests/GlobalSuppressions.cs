﻿// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S1481:Unused local variables should be removed", Justification = "Method needs to be called to check callback object in assertion below, even thought the result of this function is not used", Scope = "member", Target = "~M:Yoti.Auth.Tests.YotiClientEngineTests.ShouldAddAuthKeyHeaderToProfileRequest")]