ADR-003

1. Status

Accepted

2. Date

2026-06-29

3. Context

Forge requires a stateless authentication mechanism that supports web applications, APIs, and future mobile clients.

The authentication system should:
- Avoid server-side session storage.
- Scale horizontally.
- Integrate with ASP.NET Core authentication middleware.
- Support future refresh tokens and authorization.

4. Decision

Forge will use JWT (JSON Web Token) based authentication.

Upon successful login:
- The server issues a signed JWT access token.
- The client includes the token in the Authorization header using the Bearer scheme.
- ASP.NET Core validates the token for every authenticated request.

The JWT will initially contain:
- User Identifier
- Email Address
- JWT Identifier (JTI)

Authentication will be configured using ASP.NET Core JwtBearer middleware.

5. Rationale

JWT provides:

- Stateless authentication.
- Excellent performance.
- Native ASP.NET Core support.
- Easy integration with Swagger.
- Scalability across multiple servers.
- Future support for role and permission claims.

6. Alternatives Considered

Session-based Authentication
Rejected because it requires server-side session storage and does not scale well.

Cookie Authentication
Rejected because Forge is designed primarily as an API backend for SPA and future mobile clients.

OAuth/OpenID Connect
Rejected for the initial version since external identity providers are not currently required.

7. Consequences

Positive

- Stateless authentication.
- No server-side session management.
- Easy API consumption.
- Supports future authorization features.

Negative

- Access tokens cannot be revoked before expiry.
- Requires Refresh Tokens for long-lived sessions.

8. Implementation Notes

Implemented using:
- JwtBearer Authentication
- JwtTokenService
- CurrentUserService
- Swagger JWT Authorization

9. Future Considerations

- Refresh Tokens
- Role-based Authorization
- Permission-based Authorization
- Multi-device Sessions
- Two-Factor Authentication