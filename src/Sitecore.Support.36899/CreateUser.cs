// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CreateUser.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2017
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sitecore.Support.Commerce.Engine.Connect.Pipelines.Customers
{
  using System.Linq;
  using Diagnostics;
  using Sitecore.Support.Commerce.Engine.Connect.Pipelines;
  using Sitecore.Commerce.Services.Customers;
  using Sitecore.Commerce.Engine.Connect.Pipelines;
  using Sitecore.Commerce.Engine;
  using Sitecore.Commerce.Plugin.Customers;
  using Sitecore.Commerce;
  using Sitecore.Commerce.Pipelines;
  using Sitecore.Commerce.Entities.Customers;
  using System;
  using Sitecore.Commerce.Entities;

  /// <summary>
  /// Adds Commerce Engine Customer 
  /// </summary>   
  public class CreateUser : PipelineProcessor
  {

    public CreateUser(IEntityFactory entityFactory)
    {
      this.entityFactory = entityFactory;
    }

    // Fields
    private readonly IEntityFactory entityFactory;
    /// <summary>
    /// Processes the specified arguments.
    /// </summary>
    /// <param name="args">The arguments.</param>      
    public override void Process(ServicePipelineArgs args)
    {
      CreateUserRequest request;
      CreateUserResult result;
      Sitecore.Support.Commerce.Engine.Connect.Pipelines.PipelineUtility.ValidateArguments(args, out request, out result);

      Assert.IsNotNull(request.Email, "request.Email");
      Assert.IsNotNull(request.Password, "request.Password");

      if (result.CommerceUser!=null)
      {
        return;
      }
      else
      {
        Container container = this.GetContainer(request.Shop.Name, string.Empty, "", "", args.Request.CurrencyCode, null);
        var view = this.GetEntityView(container, string.Empty, string.Empty, "Details", "AddCustomer", result);
        if (!result.Success)
        {
          return;
        }

        view.Properties.FirstOrDefault(p => p.Name.Equals("Email")).Value = request.Email;
        view.Properties.FirstOrDefault(p => p.Name.Equals("Password")).Value = request.Password;
        view.Properties.FirstOrDefault(p => p.Name.Equals("AccountStatus")).Value = "ActiveAccount";

        var command = this.DoAction(container, view, result);

        var commerceUser = this.entityFactory.Create<CommerceUser>("CommerceUser");
        commerceUser.Email = request.Email;
        commerceUser.UserName = request.UserName;
        commerceUser.ExternalId = command.Models.OfType<CustomerAdded>().FirstOrDefault().CustomerId;


        result.CommerceUser = commerceUser;
        request.Properties.Add(new PropertyItem { Key = "UserId", Value = result.CommerceUser.ExternalId });
        base.Process(args);
      }
    }

  }
}
