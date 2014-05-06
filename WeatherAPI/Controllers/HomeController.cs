using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Web.Mvc;
using ForecastIO;

namespace WeatherAPI.Controllers
{
	public class HomeController : Controller
	{
		public String Index()
		{
			return "Use api/values/latitude,longitude";
		}

		public String WeatherService()
		{
			return "hello world";
		}
	}
}
