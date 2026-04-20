namespace Yoti.Auth.DocScan.Session.Create
{
    /// <summary>
    /// Valid values for screens that can be suppressed in the IDV flow
    /// by passing them to <see cref="SdkConfigBuilder.WithSuppressedScreens(System.Collections.Generic.List{string})"/>
    /// </summary>
    public static class SuppressedScreen
    {
        public const string IdDocumentEducation = "ID_DOCUMENT_EDUCATION";
        public const string IdDocumentRequirements = "ID_DOCUMENT_REQUIREMENTS";
        public const string SupplementaryDocumentEducation = "SUPPLEMENTARY_DOCUMENT_EDUCATION";
        public const string ZoomLivenessEducation = "ZOOM_LIVENESS_EDUCATION";
        public const string StaticLivenessEducation = "STATIC_LIVENESS_EDUCATION";
        public const string FaceCaptureEducation = "FACE_CAPTURE_EDUCATION";
        public const string FlowCompletion = "FLOW_COMPLETION";
    }
}
