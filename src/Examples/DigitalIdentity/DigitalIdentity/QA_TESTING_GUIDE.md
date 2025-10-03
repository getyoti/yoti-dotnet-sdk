# QA Testing Guide for CreateQrCode Endpoint

## Overview
The CreateQrCode endpoint has been added to support QA testing of the QR code creation functionality in the Digital Identity API.

## Proper Testing Workflow

### Step 1: Create a Digital Identity Session
First, you need to create a valid Digital Identity session to get a session ID:

**Endpoint:** `GET /generate-share`

**Response:** You'll get a session ID that starts with `ss.v2.`

Example response will show the session ID in the UI or logs.

### Step 2: Create QR Code Using Session ID
Use the session ID from Step 1 to create a QR code:

**Endpoint:** `POST /create-qr/{sessionId}`
**Method:** POST
**URL Example:** `POST /create-qr/ss.v2.abc123def456...`

## Expected Session ID Format
✅ **Valid:** `ss.v2.` followed by additional characters (e.g., `ss.v2.abc123def456...`)
❌ **Invalid:** `0`, `123`, `test`, or any string not starting with `ss.v2.`

## Common Issues and Solutions

### Issue: "UNKNOWN_SESSION" Error
**Error Message:** `Invalid session ID '0' - 'value must start with 'ss.v2.'`

**Cause:** Using an invalid session ID (like `'0'`) instead of a real session ID from the Digital Identity API.

**Solution:** 
1. First call `/generate-share` to create a session and get a valid session ID
2. Use that session ID (starting with `ss.v2.`) in the `/create-qr/{sessionId}` call

### Issue: Session ID Format Validation
The endpoint now includes validation to help identify format issues:

**If session ID doesn't start with `ss.v2.`:**
```json
{
  "sessionId": "0",
  "success": false,
  "error": "Invalid session ID format",
  "message": "Session ID must start with 'ss.v2.'. Use /generate-share endpoint first to get a valid session ID.",
  "expectedFormat": "ss.v2.xxxxx..."
}
```

## Success Response
When everything works correctly, you'll get:
```json
{
  "sessionId": "ss.v2.abc123...",
  "qrId": "51ce09b10da75c5b7da9cb1773c8f388",
  "qrUri": "https://...",
  "success": true,
  "message": "QR code created successfully"
}
```

## Testing Tips
1. **Always start with `/generate-share`** to get a valid session ID
2. **Copy the exact session ID** from the Digital Identity session creation
3. **Use POST method** for the `/create-qr/{sessionId}` endpoint
4. **Check the logs** if you're unsure about the session ID format

## SDK Manipulation
The Yoti .NET SDK does not manipulate or transform session IDs. The session ID you pass to the CreateQrCode method is sent directly to the Yoti API. If you're getting format errors, it means the session ID being passed doesn't match what the Yoti API expects.