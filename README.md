# Using Azure Keyvault From a Mobile App - Don't Do It!

🤣 Lol ... that title is kind of misleading. I'm not advising to _not_ use Keyvault when using a mobile app. But I am advising that there are better ways to integrate your application with Keyvault than to hardcode the vault's credentials directly into your mobile/Xamarin application!

And I aim to show you how to do just that with this sample!

## Deployment Instructions

So ... if you want to follow along, step by step - go ahead and create a [free Azure subscription here](https://azure.microsoft.com/free?WT.mc_id=mobile-0000-masoucou), if you don't already have one.

Hit this button and all the resources will be deployed for you!

[![Deploy to Azure](https://azuredeploy.net/deploybutton.png)](https://azuredeploy.net/)

You'll still have to deploy the Functions app. So go ahead and clone this repo. All of the Functions code can be found in the `src\functions` folder. Build it and and follow [these instructions](https://docs.microsoft.com/azure/azure-functions/deployment-zip-push?WT.mc_id=mobile-0000-masoucou) to deploy it to the Functions app you created above.

The Xamarin app, located in the `src\mobile` folder will build A-OK. Load it up in Visual Studio or Visual Studio for Mac, and you'll be on your way.

You can read the article on my blog, or just keep on reading below ... and if you have any questions, reach out at <a href="https://twitter.com/codemillmatt">@codemillmatt</a> anytime!

## The Article...

Don't hardcode connection strings - or any secrets - in your apps! And for all that's holy - encrypt those secrets!

Like most other advice that I get (floss... exercise... shower more than once per week...) that seemingly easy to follow best practice of security guidance doesn't get followed very often.

Why? Because it's an extra step in the development cycle. Why go through the hassle of encrypting, storing, having to come up with a means to distribute my app's secrets to other devs on my team... when I could just have them sitting around somewhere in plain text? What are the odds that somebody will decompile my app to get at them anyway?

Just like why would I exercise when I could sit around, eat Pringles, and watch re-runs of SpongeBob SquarePants? What are the odds that I'm going to die of a heart attack today?

![watching spongebob](https://res.cloudinary.com/code-mill-technologies-inc/image/upload/bo_1px_solid_rgb:000000,c_scale,h_500/v1552429919/PNG_image-EF444747D594-1_si76os.png)

Well ... eventually luck runs out. Someday, something bad will happen to my app's secrets even if the app doesn't get cracked (like I commit the secrets file to GitHub). Just like someday I'll regret skipping all of those workouts to watch marathons of the Golden Girls. (Lol ... like that would ever be a regret.)

But there has to be a way ... there has to be a way to keep the app's secrets safe, all the while following best practices, _and_ not making our lives too difficult ... right? Right?

## Enter Azure Keyvault

[Azure Keyvault](https://docs.microsoft.com/azure/key-vault/key-vault-overview?WT.mc_id=mobile-0000-masoucou) provides a centralized place to store not only your application's secrets, but also can be used for key and [certificate management](https://docs.microsoft.com/azure/key-vault/certificate-scenarios?WT.mc_id=mobile-0000-masoucou) too.

> Keyvault puts all secrets in one spot ... almost as good as a plain text file, right? 😉

Keyvault's secrets are essentially a key/value store - but with lots of polish on top of them. At a minimum, you create a secret within KeyVault, give it a name and then place the (secret) value in it.

The data/secret is encrypted both at rest and in transmission (and Microsoft never sees the data contained within). You can monitor the access & use of each secret. And access to secrets is controlled by Active Directory.

You can version secrets - so if the connection string changes you can set the existing value as old and create a new value, with the connection string, all within the same key. 

You also can disable keys, set activation and deactivation dates. 

And each secret in Keyvault gets its own URI which it can be referenced by.

So it's cool, right? Application secrets in a single spot, encryption by default, and there are even [Microsoft provided SDKs](https://docs.microsoft.com/dotnet/api/overview/azure/key-vault?WT.mc_id=mobile-0000-masoucou) to get at them.

Done and done.

## Let's Use Keyvault!

So I have an app that pulls some data from [Azure Table storage](https://docs.microsoft.com/azure/cosmos-db/table-storage-overview?WT.mc_id=mobile-0000-masoucou).

It's a Xamarin.Forms app and it displays the data it finds in the Table in a `ListView` - it looks like this:

![app before refactor](https://res.cloudinary.com/code-mill-technologies-inc/image/upload/bo_1px_solid_rgb:000000,c_scale,h_700/v1552347777/Simulator_Screen_Shot_-_iPhone_XS_-_2019-03-11_at_16.39.25_okxs0b.png)

The code which grabs the data from Table storage looks like this:

<script src="https://gist.github.com/codemillmatt/9343cf6814a4545e7df7b7b5d4149977.js"></script>

There's a couple things wrong here (or as I like to say, it's demo code!) - but the big one, and the one that we're going to fix, is that the Azure Storage account's API key is hardcoded.

The fix should be fairly easy ... add the API key for Azure Storage to Keyvault, use the .NET Keyvault SDK to grab it, then continue on with the code - pretty much as is.

### So Let's Add The Secret!

[Adding the secret in Keyvault](https://docs.microsoft.com/azure/key-vault/quick-create-portal?WT.mc_id=mobile-0000-masoucou) is easy, easy, easy! (As an aside, don't you hate when people say something is easy? Everything's easy when you already know how to do it! So let's try it again....)

[Adding the secret in Keyvault](https://docs.microsoft.com/azure/key-vault/quick-create-portal?WT.mc_id=mobile-0000-masoucou) isn't too bad!

Open up your Keyvault in the portal, go to the `Secrets` option then the `Generate/Import` button.

![first step to generating a key in key vault](https://res.cloudinary.com/code-mill-technologies-inc/image/upload/bo_1px_solid_rgb:000000,c_scale,h_600/v1552408635/Screen_Shot_2019-03-12_at_9.35.53_AM_mwzemo.png)

Then you'll see this page - on there give the secret a name - and pop in the value. In this case it'll be the storage connection key.

![setting the storage connection key value](https://res.cloudinary.com/code-mill-technologies-inc/image/upload/bo_1px_solid_rgb:000000/v1552408804/Screen_Shot_2019-03-12_at_9.39.23_AM_dvvtdr.png)

Now on to using Keyvault's .NET SDK to access it from our mobile app and we'll be done!

### But... Don't Use Keyvault - In a Mobile App

Ugh... but I have some bad news ... don't use Azure Keyvault when you're building a mobile app.

At least not directly.

You see, we want to keep all app secrets off the device if possible. Even if they're never being stored and will only be in memory transiently.

To solve this particular conumdrum... we're still going to take advantage of Keyvault and all it has to offer and put the Azure Storage API key in there.

But instead of having the mobile app directly access the Table storage, we're going to have an [Azure Function](https://docs.microsoft.com/azure/azure-functions/functions-overview?WT.mc_id=mobile-0000-masoucou) do that.

This then gives us several benefits.

1. We get to use Keyvault! 🎉
2. Keyvault is accessed completely over the Azure backbone - no public internet - and same for Azure storage. _Fast!_
3. We'll end up with an API that can be used in more places than the mobile app.

OK good ... we're going to use [Function-Table storage bindings](https://docs.microsoft.com/azure/azure-functions/functions-bindings-storage-table?WT.mc_id=mobile-0000-masoucou) to get at the data. And the mobile app will call that Function.

But how does the Function access Keyvault to retrieve the Azure Storage connection string?

Hardcoding Keyvault's credentials into the Function's app settings would kind of defeat the purpose of _not_ having the Storage's connection string in the app settings.

So how does Azure Functions communicate to Azure Keyvault ... without the hardcoded credentials?

Through the wonders of the [Managed Identity](https://docs.microsoft.com/azure/app-service/overview-managed-identity?WT.mc_id=mobile-0000-masoucou).

## Managed Identity

So remember when I said that access to the Keyvault is controlled through [Active Directory](https://docs.microsoft.com/azure/key-vault/key-vault-secure-your-key-vault?WT.mc_id=mobile-0000-masoucou)?

It turns out that it's pretty straightforward to make your Function App a member of the Active Directory. And once it's Active Directory-izied you can grant the Function App access to Key Vault as easily as you can grant access to a person that's in Active Directory.

And this is what's known as Managed Identity.

### Active Directory-izing Your Function App

This [same process](https://docs.microsoft.com/azure/app-service/overview-managed-identity?WT.mc_id=mobile-0000-masoucou) goes for any App service.

It's really kinda anti-climatic.

Pop over to the Azure portal for your Function. Select the `Platform Features` tab, then the `Identity` option.

![Azure portal for enabling managed identity for azure function](https://res.cloudinary.com/code-mill-technologies-inc/image/upload/bo_1px_solid_rgb:000000/c_scale,h_600/v1552407050/Screen_Shot_2019-03-12_at_9.09.20_AM_k3bszq.png)

You have Active Directory-izied your Functions app.

Now the Functions App (which is really an App Service) will now appear in the Active Directory that your Azure subscription is a part of.

Done (with this part).

### Granting the Keyvault Permissions

The next step is to grant the new Active Directory (AD) principal - aka your Function App permission to the secrets contained within Keyvault.

Back over to Keyvault in the Azure portal we go!

You grant new permissions to AD principals through the `Access policies` option and the `Add new` button.

![Adding a new principal for key vault permissions](https://res.cloudinary.com/code-mill-technologies-inc/image/upload/bo_1px_solid_rgb:000000/c_scale,h_900/v1552409555/Screen_Shot_2019-03-12_at_9.47.45_AM_qjsqsk.png)

The next page then take a bit of explanation.

![The actual granting of permissions to key vault page](https://res.cloudinary.com/code-mill-technologies-inc/image/upload/bo_1px_solid_rgb:000000/c_scale,h_900/v1552410147/Screen_Shot_2019-03-12_at_9.55.31_AM_vl22mw.png)

The first thing you can do is `Configure from template`. This is a shortcut to select various permissions for you. Which can be seen in the various permissions dropdowns. 

I picked `Secret Management` which selected all the secret functions for me.

The next thing that needs to be done is select the principal to apply these roles to.

And to select your function app - search for its name. Eventually AD will find it. Select the name and hit `Select` and `OK` a bunch of times.

And then you're set.

## Getting the Function to Talk to Keyvault

Now to get the [Function to use Keyvault](https://docs.microsoft.com/azure/app-service/app-service-key-vault-references?WT.mc_id=mobile-0000-masoucou). The code looks like this:

<script src="https://gist.github.com/codemillmatt/ae87d15286ae73c01db97ae830a043ae.js"></script>

If you've ever created a Function that accessed Azure Table storage before, you're probably to yourself ... "the Function's code when accessing Keyvault is exactly the same as when it doesn't!"

> The Function's code when accessing Keyvault is exactly the same as when it doesn't!
> 

But how does the Function use Keyvault? The magic is in the App Settings. In particular the `KeyvaultedStorageConnection` string.

To make that particular value access Keyvault - you use this syntax:

<script src="https://gist.github.com/codemillmatt/ba67c76a5e309208eeae1eda6809c180.js"></script>

So it goes into the Function's app settings. It's the value portion of the `KeyvaultedStorageConnection` key.

And as you can tell, it uses the syntax: `@Microsoft.Keyvault(SecretUri=<A SECRET URI>)`.

Where does that `<A SECRET URI>` come from?

From the secret's panel in Keyvault, of course! Back into Keyvault into the portal you go!

Click on `Secrets` on the left-hand navigation. Then click on the secret you just created. Then by clicking on the `current version` in the resulting window, you'll see the property's of the secret.

In that window - there's a `Secret Identifier` box. That's the URI which you'll use for the `<A SECRET URI>` above.

![Finding the secret's identifer](https://res.cloudinary.com/code-mill-technologies-inc/image/upload/bo_1px_solid_rgb:000000/v1552411279/Screen_Shot_2019-03-12_at_10.19.31_AM_ohmrdw.png)

## The App's Code

At this point you're done. Well, except for getting the mobile app to call the Function of course.

The Function in this case is HTTP triggered. So it's a web call from our Xamarin app - looks like this:

<script src="https://gist.github.com/codemillmatt/87d87ca3a2ba90353a22f3cc2efce3a8.js"></script>

## That's All Folks

And finally, we've reached the end of our journey!

The mobile app no longer has secrets hardcoded in it, but yet it's not using Keyvault either! Because we don't want any secrets to land on that mobile app if we can at all avoid it.

We're still using Keyvault though, and all of the goodness it provides, and created a Function to access it - through Managed Identity. This way the Function doesn't have to have any secrets stored in it either!

Cool, cool, cool! Not too much extra work - and a ton of extra security!

Now if you'll excuse me - I think there's another rerun of SpongeBob about to air.

### A Quick Postscript - Or Using a SAS On Device

Why not use a [Storage Access Signature (SAS)](https://docs.microsoft.com/azure/storage/common/storage-dotnet-shared-access-signature-part-1?WT.mc_id=mobile-0000-masoucou) to access the Azure Storage Account? Well, from the mobile device that could totally have been done. And with that I could have still used the .NET Storage SDK.

However, to generate that SAS, I would still have needed to get direct access to the Storage account through the full connection string somehow. And that would be done in the same manner as above. Except instead of returning data, the Function would have returned an SAS token.

