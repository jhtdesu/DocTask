# Upload File API Documentation

## Overview
This API provides file upload functionality for the DocTask application. Files are stored in the `@UploadFile/` directory and metadata is stored in the database.

## Endpoints

### 1. Upload File
**POST** `/api/v1/upload/file`

Upload a file to the server.

**Headers:**
- `Authorization: Bearer <token>` (required)

**Request Body:**
- `file`: The file to upload (multipart/form-data)
- `description`: Optional description of the file

**Response:**
```json
{
  "success": true,
  "data": {
    "fileId": 1,
    "fileName": "document.pdf",
    "filePath": "C:\\Repository\\DocTask\\DocTask.Data\\UploadFile\\unique-filename.pdf",
    "uploadedBy": "user-id",
    "uploadedAt": "2024-01-01T12:00:00Z",
    "fileSize": 1024000,
    "contentType": "application/pdf"
  },
  "message": "File uploaded successfully"
}
```

### 2. Get File Information
**GET** `/api/v1/upload/file/{fileId}`

Get information about a specific file.

**Headers:**
- `Authorization: Bearer <token>` (required)

**Response:**
```json
{
  "success": true,
  "data": {
    "fileId": 1,
    "fileName": "document.pdf",
    "filePath": "C:\\Repository\\DocTask\\DocTask.Data\\UploadFile\\unique-filename.pdf",
    "uploadedBy": "user-id",
    "uploadedAt": "2024-01-01T12:00:00Z",
    "fileSize": 1024000,
    "contentType": "application/pdf"
  }
}
```

### 3. Get User Files
**GET** `/api/v1/upload/files`

Get all files uploaded by the current user.

**Headers:**
- `Authorization: Bearer <token>` (required)

**Response:**
```json
{
  "success": true,
  "data": [
    {
      "fileId": 1,
      "fileName": "document.pdf",
      "filePath": "C:\\Repository\\DocTask\\DocTask.Data\\UploadFile\\unique-filename.pdf",
      "uploadedBy": "user-id",
      "uploadedAt": "2024-01-01T12:00:00Z",
      "fileSize": 1024000,
      "contentType": "application/pdf"
    }
  ]
}
```

### 4. Download File
**GET** `/api/v1/upload/file/{fileId}/download`

Download a file by its ID.

**Headers:**
- `Authorization: Bearer <token>` (required)

**Response:**
- File content with appropriate Content-Type header

### 5. Delete File
**DELETE** `/api/v1/upload/file/{fileId}`

Delete a file (only files uploaded by the current user).

**Headers:**
- `Authorization: Bearer <token>` (required)

**Response:**
```json
{
  "success": true,
  "data": true,
  "message": "File deleted successfully"
}
```

## File Storage
- Files are stored in the `DocTask.Data/UploadFile/` directory
- Each file is given a unique filename using GUID to prevent conflicts
- Original filename is preserved in the database
- File size limit: 10MB per file

## Supported File Types
The API supports various file types including:
- PDF documents
- Microsoft Office documents (Word, Excel, PowerPoint)
- Images (JPEG, PNG, GIF)
- Text files
- And more (with appropriate MIME type detection)

## Error Handling
All endpoints return appropriate HTTP status codes and error messages in the response body:

```json
{
  "success": false,
  "error": "Error message describing what went wrong"
}
```

## Authentication
All endpoints require JWT authentication. Include the Bearer token in the Authorization header.
