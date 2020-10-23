namespace Yoti.Auth.Constants
{
    public static class DocScanConstants
    {
        public const string IdDocumentAuthenticity = "ID_DOCUMENT_AUTHENTICITY";
        public const string IdDocumentComparison = "ID_DOCUMENT_COMPARISON";
        public const string IdDocumentTextDataCheck = "ID_DOCUMENT_TEXT_DATA_CHECK";
        public const string IdDocumentTextDataExtraction = "ID_DOCUMENT_TEXT_DATA_EXTRACTION";
        public const string IdDocumentFaceMatch = "ID_DOCUMENT_FACE_MATCH";
        public const string SupplementaryDocumentTextDataCheck = "SUPPLEMENTARY_DOCUMENT_TEXT_DATA_CHECK";
        public const string SupplementaryDocumentTextDataExtraction = "SUPPLEMENTARY_DOCUMENT_TEXT_DATA_EXTRACTION";
        public const string Liveness = "LIVENESS";
        public const string Zoom = "ZOOM";

        public const string Camera = "CAMERA";
        public const string CameraAndUpload = "CAMERA_AND_UPLOAD";

        public const string ResourceUpdate = "RESOURCE_UPDATE";
        public const string TaskCompletion = "TASK_COMPLETION";
        public const string CheckCompletion = "CHECK_COMPLETION";
        public const string SessionCompletion = "SESSION_COMPLETION";

        public const string Always = "ALWAYS";
        public const string Fallback = "FALLBACK";
        public const string Never = "NEVER";

        public const string Desired = "DESIRED";
        public const string Ignore = "IGNORE";

        public const string IdDocument = "ID_DOCUMENT";
        public const string OrthogonalRestrictions = "ORTHOGONAL_RESTRICTIONS";
        public const string DocumentRestrictions = "DOCUMENT_RESTRICTIONS";
        public const string SupplementaryDocument = "SUPPLEMENTARY_DOCUMENT";

        public const string IncludeList = "WHITELIST";
        public const string ExcludeList = "BLACKLIST";

        public const string ProofOfAddress = "PROOF_OF_ADDRESS";
    }
}