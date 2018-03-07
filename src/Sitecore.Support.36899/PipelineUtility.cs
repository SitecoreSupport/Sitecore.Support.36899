using Sitecore.Commerce.Pipelines;
using Sitecore.Commerce.Services;
using Sitecore.Diagnostics;
using Sitecore.Pipelines;
using System;

namespace Sitecore.Support.Commerce.Engine.Connect.Pipelines
{
  public static class PipelineUtility
  {
    internal static void ValidateArguments<TRequest, TResult>(ServicePipelineArgs args, out TRequest request, out TResult result) where TRequest : ServiceProviderRequest where TResult : ServiceProviderResult
    {
      Assert.ArgumentNotNull(args, "args");
      Assert.ArgumentNotNull(args.Request, "args.Request");
      Assert.ArgumentNotNull(args.Request.RequestContext, "args.Request.RequestContext");
      Assert.ArgumentNotNull(args.Result, "args.Result");
      request = (args.Request as TRequest);
      result = (args.Result as TResult);
      Assert.IsNotNull(request, "The parameter args.Request was not of the expected type.  Expected {0}.  Actual {1}.", new object[]
      {
                typeof(TRequest).Name,
                args.Request.GetType().Name
      });
      Assert.IsNotNull(result, "The parameter args.Result was not of the expected type.  Expected {0}.  Actual {1}.", new object[]
      {
                typeof(TResult).Name,
                args.Result.GetType().Name
      });
    }
  }
}
