# Global Error Handling

Using the built-in error endpoint middleware and overriding the **DefaultProblemDetailsFactory** (which is sealed) by creating **AuthFromScratchProblemDetailsFactory** and modifying the **ApplyProblemDetailsDefaults** method.


## Middleware Configuration
```csharp
app.UseExceptionHandler("/error");
```

## Endpoint
```csharp
[ApiController]
public  class  ErrorsController: ControllerBase
{
	[Route("/error")]
	public  IActionResult  Error()
	{
		Exception?  exception  =  HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;
		
		return  Problem(title: exception?.Message, statusCode: 400);
	}
}
```

## AuthFromScratchProblemDetailsFactory

Because the **DefaultProblemDetailsFactory** class from **aspnetcore** is sealed and we cannot derive from it we are implementing our own **AuthFromScratchProblemDetailsFactory** which is just the default one but with a slight modification in the **ApplyProblemDetailsDefaults** method.
```csharp
// Add this line to ApplyProblemDetailsDefaults()
problemDetails.Extensions.Add("customProperty",  "customValue");
```
Method:
```csharp
private  void  ApplyProblemDetailsDefaults(HttpContext  httpContext, ProblemDetails  problemDetails, int  statusCode)
{
	problemDetails.Status  ??=  statusCode;
	
	if (_options.ClientErrorMapping.TryGetValue(statusCode, out  var  clientErrorData))
	{	
		problemDetails.Title  ??=  clientErrorData.Title;
		problemDetails.Type  ??=  clientErrorData.Link;
	}	  

	var  traceId  =  Activity.Current?.Id  ??  httpContext?.TraceIdentifier;

	if (traceId  !=  null)
	{
		problemDetails.Extensions["traceId"] =  traceId;
	}	  
	// Adding custom extensions
	problemDetails.Extensions.Add("customProperty", "customValue");	  

	_configure?.Invoke(new() { HttpContext  =  httpContext!, ProblemDetails  =  problemDetails });
}
```