# Tuttofila SPA
Documentation

# Web application

## Web Project creation - v0.1

* Create an ASP.NET Web Application (.NET Framework 4.7.1) with Empty template
* Add a class library
* Exclude bower packages from gitignore
    /TuttofilaSPA.Web/bower_components/**/*.*
* Install mandatory packages in web project and class library


## Db tables - v0.2

* Add Sala and Servizio table
* Db context creation


## Owin - v0.3

https://docs.microsoft.com/en-us/aspnet/web-api/overview/hosting-aspnet-web-api/use-owin-to-self-host-web-api <br/>
https://autofac.readthedocs.io/en/latest/getting-started/index.html#application-startup <br/>
https://docs.microsoft.com/it-it/aspnet/web-api/overview/odata-support-in-aspnet-web-api/odata-v4/create-an-odata-v4-endpoint

* Context factory implementation
* Core autofac module implementation
* Startup file creation
* web api/odata configuration


## Odata controllers - v0.4

* Sale controller implementation
* Servizi controller implementation


# Angularjs application

## Client packages installation - v0.5

https://bower.io/

* Adding bower.json file
* Mandatory packages installation


## Main page - v0.6

https://docs.angularjs.org/tutorial/step_00 <br/>
https://github.com/angular-ui/ui-router/wiki <br/>
https://ui-router.github.io/ng1/tutorial/helloworld <br/>

* app.js implementation
* mainModule.js implementation


## Sale module - v0.7

https://www.odata.org/getting-started/basic-tutorial/ <br/>

* SaleFactory implementation
* Sale controllers implementation
* html pages implementation
* Adding Sale module to main module


## Servizio module - v0.8

* ServiziFactory implementation
* Servizi controllers implementation
* html pages implementation
* Adding Servizi module to main module


# Rabbitmq

## Publisher/Consumer - v0.9

https://www.rabbitmq.com/tutorials/amqp-concepts.html <br/>
https://www.rabbitmq.com/tutorials/tutorial-one-dotnet.html <br/>
https://www.rabbitmq.com/tutorials/tutorial-two-dotnet.html <br/>
https://www.rabbitmq.com/tutorials/tutorial-two-dotnet.html <br/>

* Publisher implementation
* Consumer implementation


## Sportelli controller - v0.9

* Adding Pubblica action
* Adding Consuma action
* Adding RestituisciServiziChiamati action


## Sportelli module - v1.0

* SportelliFactory implementation
* Sportelli controllers implementation
* html page implementation
* Adding Sportelli module to main module

