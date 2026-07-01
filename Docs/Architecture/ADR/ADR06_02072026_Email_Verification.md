# ADR-006: Email Verification

## Status

Accepted

---

## Overview

Forge verifies user email addresses using secure one-time verification tokens.

The implementation is independent from password recovery and refresh tokens.

---

## Components

### Entity

EmailVerificationToken

| Property | Description |
|----------|-------------|
| Id | Primary key |
| TokenHash | SHA-256 hash of verification token |
| ExpiresAt | Expiration timestamp |
| UsedAt | Verification timestamp |
| UserId | Owner of the token |

---

### Service

EmailVerificationService

Responsibilities:

- Generate verification token
- Hash token
- Validate token
- Mark token as used

---

## Verification Flow

```
Register
    │
    ▼
Generate Verification Token
    │
    ▼
Hash Token
    │
    ▼
Store Hash
    │
    ▼
Send Email (Mocked)
    │
    ▼
User Clicks Link
    │
    ▼
Validate Token
    │
    ▼
Mark Token Used
    │
    ▼
User.IsEmailVerified = true
```

---

## Resend Verification Flow

```
Authenticated User
    │
    ▼
Resend Verification
    │
    ▼
Already Verified?
    │
    ├── Yes
    │      │
    │      └── Business Exception
    │
    ▼
Generate Token
    │
    ▼
Store Token
    │
    ▼
Send Email (Mocked)
```

---

## Security Considerations

### Token Storage

Only the SHA-256 hash is stored.

Plain tokens are never persisted.

---

### Token Lifetime

Default:

- 24 Hours

---

### One-Time Usage

Verification tokens can only be used once.

After verification:

- UsedAt is populated
- User.IsEmailVerified = true

---

### Multiple Tokens

If multiple verification emails are generated:

- Each token remains independently valid until:
  - Expired
  - Used

Future versions may invalidate previous verification tokens automatically.

---

## Future Improvements

- Automatic email verification after OAuth login
- HTML email templates
- Email change verification
- Verification reminders
- Scheduled cleanup of expired verification tokens