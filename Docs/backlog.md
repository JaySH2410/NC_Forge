AUTH-001
Title: Standardize ASP.NET Core model binding errors

Priority: Low

Description:
Replace the default ProblemDetails response for model binding and JSON deserialization errors with Forge's standard ApiResponse format.

Reason Deferred:
The frontend can handle both response formats initially. This is not blocking authentication or future capabilities.