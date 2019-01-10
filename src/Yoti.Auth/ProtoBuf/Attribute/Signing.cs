// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: Signing.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Yoti.Auth.ProtoBuf.Attribute {

  /// <summary>Holder for reflection information generated from Signing.proto</summary>
  public static partial class SigningReflection {

    #region Descriptor
    /// <summary>File descriptor for Signing.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static SigningReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "Cg1TaWduaW5nLnByb3RvEg1hdHRycHViYXBpX3YxGhFDb250ZW50VHlwZS5w",
            "cm90byKqAQoQQXR0cmlidXRlU2lnbmluZxIMCgRuYW1lGAEgASgJEg0KBXZh",
            "bHVlGAIgASgMEjAKDGNvbnRlbnRfdHlwZRgDIAEoDjIaLmF0dHJwdWJhcGlf",
            "djEuQ29udGVudFR5cGUSGgoSYXJ0aWZhY3Rfc2lnbmF0dXJlGAQgASgMEhAK",
            "CHN1Yl90eXBlGAUgASgJEhkKEXNpZ25lZF90aW1lX3N0YW1wGAYgASgMQm8K",
            "JGNvbS55b3RpLmFwaS5jbGllbnQuc3BpLnJlbW90ZS5wcm90b0IMU2lnbmlu",
            "Z1Byb3RvWg15b3RpcHJvdG9hdHRyqgIcWW90aS5BdXRoLlByb3RvQnVmLkF0",
            "dHJpYnV0ZcoCCkF0dHJwdWJhcGliBnByb3RvMw=="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::Yoti.Auth.Protobuf.Attribute.ContentTypeReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Yoti.Auth.ProtoBuf.Attribute.AttributeSigning), global::Yoti.Auth.ProtoBuf.Attribute.AttributeSigning.Parser, new[]{ "Name", "Value", "ContentType", "ArtifactSignature", "SubType", "SignedTimeStamp" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  public sealed partial class AttributeSigning : pb::IMessage<AttributeSigning> {
    private static readonly pb::MessageParser<AttributeSigning> _parser = new pb::MessageParser<AttributeSigning>(() => new AttributeSigning());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<AttributeSigning> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Yoti.Auth.ProtoBuf.Attribute.SigningReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public AttributeSigning() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public AttributeSigning(AttributeSigning other) : this() {
      name_ = other.name_;
      value_ = other.value_;
      contentType_ = other.contentType_;
      artifactSignature_ = other.artifactSignature_;
      subType_ = other.subType_;
      signedTimeStamp_ = other.signedTimeStamp_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public AttributeSigning Clone() {
      return new AttributeSigning(this);
    }

    /// <summary>Field number for the "name" field.</summary>
    public const int NameFieldNumber = 1;
    private string name_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Name {
      get { return name_; }
      set {
        name_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "value" field.</summary>
    public const int ValueFieldNumber = 2;
    private pb::ByteString value_ = pb::ByteString.Empty;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pb::ByteString Value {
      get { return value_; }
      set {
        value_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "content_type" field.</summary>
    public const int ContentTypeFieldNumber = 3;
    private global::Yoti.Auth.Protobuf.Attribute.ContentType contentType_ = 0;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Yoti.Auth.Protobuf.Attribute.ContentType ContentType {
      get { return contentType_; }
      set {
        contentType_ = value;
      }
    }

    /// <summary>Field number for the "artifact_signature" field.</summary>
    public const int ArtifactSignatureFieldNumber = 4;
    private pb::ByteString artifactSignature_ = pb::ByteString.Empty;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pb::ByteString ArtifactSignature {
      get { return artifactSignature_; }
      set {
        artifactSignature_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "sub_type" field.</summary>
    public const int SubTypeFieldNumber = 5;
    private string subType_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string SubType {
      get { return subType_; }
      set {
        subType_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "signed_time_stamp" field.</summary>
    public const int SignedTimeStampFieldNumber = 6;
    private pb::ByteString signedTimeStamp_ = pb::ByteString.Empty;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pb::ByteString SignedTimeStamp {
      get { return signedTimeStamp_; }
      set {
        signedTimeStamp_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as AttributeSigning);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(AttributeSigning other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Name != other.Name) return false;
      if (Value != other.Value) return false;
      if (ContentType != other.ContentType) return false;
      if (ArtifactSignature != other.ArtifactSignature) return false;
      if (SubType != other.SubType) return false;
      if (SignedTimeStamp != other.SignedTimeStamp) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Name.Length != 0) hash ^= Name.GetHashCode();
      if (Value.Length != 0) hash ^= Value.GetHashCode();
      if (ContentType != 0) hash ^= ContentType.GetHashCode();
      if (ArtifactSignature.Length != 0) hash ^= ArtifactSignature.GetHashCode();
      if (SubType.Length != 0) hash ^= SubType.GetHashCode();
      if (SignedTimeStamp.Length != 0) hash ^= SignedTimeStamp.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (Name.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(Name);
      }
      if (Value.Length != 0) {
        output.WriteRawTag(18);
        output.WriteBytes(Value);
      }
      if (ContentType != 0) {
        output.WriteRawTag(24);
        output.WriteEnum((int) ContentType);
      }
      if (ArtifactSignature.Length != 0) {
        output.WriteRawTag(34);
        output.WriteBytes(ArtifactSignature);
      }
      if (SubType.Length != 0) {
        output.WriteRawTag(42);
        output.WriteString(SubType);
      }
      if (SignedTimeStamp.Length != 0) {
        output.WriteRawTag(50);
        output.WriteBytes(SignedTimeStamp);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (Name.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Name);
      }
      if (Value.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeBytesSize(Value);
      }
      if (ContentType != 0) {
        size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) ContentType);
      }
      if (ArtifactSignature.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeBytesSize(ArtifactSignature);
      }
      if (SubType.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(SubType);
      }
      if (SignedTimeStamp.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeBytesSize(SignedTimeStamp);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(AttributeSigning other) {
      if (other == null) {
        return;
      }
      if (other.Name.Length != 0) {
        Name = other.Name;
      }
      if (other.Value.Length != 0) {
        Value = other.Value;
      }
      if (other.ContentType != 0) {
        ContentType = other.ContentType;
      }
      if (other.ArtifactSignature.Length != 0) {
        ArtifactSignature = other.ArtifactSignature;
      }
      if (other.SubType.Length != 0) {
        SubType = other.SubType;
      }
      if (other.SignedTimeStamp.Length != 0) {
        SignedTimeStamp = other.SignedTimeStamp;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 10: {
            Name = input.ReadString();
            break;
          }
          case 18: {
            Value = input.ReadBytes();
            break;
          }
          case 24: {
            contentType_ = (global::Yoti.Auth.Protobuf.Attribute.ContentType) input.ReadEnum();
            break;
          }
          case 34: {
            ArtifactSignature = input.ReadBytes();
            break;
          }
          case 42: {
            SubType = input.ReadString();
            break;
          }
          case 50: {
            SignedTimeStamp = input.ReadBytes();
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
