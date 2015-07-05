# Qurvey App
A small App and its Backend for creating and evaluating surveys spontaneously in the lecture. Designed to work using the RWTH Aachen Universities L2P (elearning platform) and OAuth for authorizing and course room selection.
The App was written in a short practical lab during the summer term 2015.

## License
The Source Code of the Qurvey App and its Backend is licensed under the MIT License.

## Frameworks/Dependencies
This project uses the Xamarin-Framework (Xamarin.iOS, Xamarin.Android) and the Windows Phone SDK.
It is a Shared Project, needing at least Visual Studio 2013 Update 2. Xamarin/WindowsPhone SDK may have further requirements.
Additionally, json.net (MIT-License) is used for JSON-(De)Serialization.
A set of Microsofts extended .NET packaged is used (e.g. System.net.http).

## OAuth/L2P
The App uses the L2P elearning platform and the OAuth Service of the RWTH Aachen University. By changing the API-classes, the system can be adopted to other systems.

## Projects
The whole Application consists of multiple projects:

* Qurvey - A shared project containing Xamarin.Forms UI and business logic of the app clients
* Qurvey.Shared - A shared project containing the Models and Data Structured that is used for the clients and the Backend
* Qurvey.Droid - The Xamarin.Android Client
* Qurvey.iOS - The Xamarin.iOS Client
* Qurvey.WinPhone - The WindowsPhone Client
* Qurvey.Backend - The Backend providing a REST-API and can be used using the Microsoft Azure Service
* Qurvey.Backend.Test - Tests for the Backend