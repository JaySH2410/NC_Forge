ADR-004

1. Status

Accepted

2. Date

2026-06-29

3. Context

JWT access tokens are intentionally short-lived.

Without a refresh mechanism, users would need to log in repeatedly after the access token expires.

Forge requires a secure mechanism for maintaining authenticated sessions without compromising security.

4. Decision

Forge will implement Refresh Tokens stored in the database.

Characteristics:

- Refresh Tokens are randomly generated.
- Stored securely in the database.
- Linked to a specific user.
- Have their own expiration date.
- Can be revoked.
- Used only to obtain new access tokens.

Access Tokens remain stateless JWTs.

5. Rationale

Database-backed refresh tokens provide:

- Immediate revocation.
- Secure logout.
- Session tracking.
- Multiple device support.
- Better auditing.

6. Alternatives Considered

Long-lived JWT Access Tokens
Rejected because compromised tokens remain valid until expiry.

Server-side Sessions
Rejected because Forge uses stateless API authentication.

In-memory Refresh Tokens
Rejected because tokens would be lost after application restart.

7. Consequences

Positive

- Better security.
- Controlled session lifetime.
- Secure logout.
- Future support for multiple devices.

Negative

- Additional database table.
- Slightly increased implementation complexity.

8. Implementation Notes

RefreshToken entity will store:

- UserId
- Token
- ExpiresAt
- CreatedAt
- RevokedAt
- ReplacedByToken
- ReasonRevoked

The authentication flow becomes:

Login
→ Access Token
→ Refresh Token

Refresh
→ New Access Token
→ New Refresh Token

Logout
→ Refresh Token Revoked

9. Future Considerations

- Refresh Token Rotation
- Device Tracking
- Session Management
- Concurrent Session Limits
- Automatic Cleanup of Expired Tokens