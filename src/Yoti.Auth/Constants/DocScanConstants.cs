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
        public const string ThirdPartyIdentity = "THIRD_PARTY_IDENTITY";
        public const string ThirdPartyIdentityFraudOne = "THIRD_PARTY_IDENTITY_FRAUD_1";
        public const string WatchlistScreening = "WATCHLIST_SCREENING";
        public const string WatchlistAdvancedCa = "WATCHLIST_ADVANCED_CA";

        public const string WithYotiAccount = "WITH_YOTI_ACCOUNT";
        public const string WithCustomAccount = "WITH_CUSTOM_ACCOUNT";
        public const string TypeList = "TYPE_LIST";
        public const string Profile = "PROFILE";
        public const string Exact = "EXACT";
        public const string Fuzzy = "FUZZY";

        public const string Liveness = "LIVENESS";
        public const string Zoom = "ZOOM";
        public const string Static = "STATIC";
        public const string FaceCapture = "FACE_CAPTURE";

        public const string Camera = "CAMERA";
        public const string CameraAndUpload = "CAMERA_AND_UPLOAD";

        public const string ResourceUpdate = "RESOURCE_UPDATE";
        public const string TaskCompletion = "TASK_COMPLETION";
        public const string CheckCompletion = "CHECK_COMPLETION";
        public const string SessionCompletion = "SESSION_COMPLETION";

        public const string Sanctions = "SANCTIONS";
        public const string AdverseMedia = "ADVERSE-MEDIA";

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

        public const string Basic = "BASIC";
        public const string Bearer = "BEARER";

        public const string MultiPartBoundary = "yoti-doc-scan-boundary";
        public const string UploadFaceCaptureImageBinaryContentName = "binary-content";
        public const string UploadFaceCaptureImageFileName = "face-capture-image";

        public const string MimeTypeJpg = "image/jpeg";
        public const string MimeTypePng = "image/png";

        public const string EndUser = "END_USER";
        public const string RelyingBusiness = "RELYING_BUSINESS";
        public const string Ibv = "IBV";

        public const string Reclassification = "RECLASSIFICATION";
        public const string Generic = "GENERIC";
    }
}