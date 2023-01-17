using System;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nop.Core.Infrastructure;
using Nop.Plugin.Widgets.TorchDesign_StoreLocator;

public class GoogleMapsAPIHelpersCS
{
    public static XElement GetGeocodingSearchResults(string address)
    {
        var googleAPISetting = EngineContext.Current.Resolve<StoreLocatorSettings>();
        // Use the Google Geocoding service to get information about the user-entered address
        // See http://code.google.com/apis/maps/documentation/geocoding/index.html for more info...
        //var url = String.Format("https://maps.google.com/maps/api/geocode/xml?address={0}&sensor=false&zoom=12&key={1}", HttpUtility.UrlEncode(address), googleAPISetting.GoogleAPIKey);
        var url = string.Format("https://maps.googleapis.com/maps/api/geocode/xml?address={0}&zoom=12&key={1}", HttpUtility.UrlEncode(address), googleAPISetting.GoogleAPIKey);

        // Load the XML into an XElement object (whee, LINQ to XML!)
        var results = XElement.Load(url);

        // Check the status
        var status = results.Element("status").Value;
        if (status == "OVER_QUERY_LIMIT")
            throw new ApplicationException("You have exceeded your daily request quota for this API. If you did not set a custom daily request quota, verify your project has an active billing account: http://g.co/dev/maps-no-account " + status);

        if (status != "OK" && status != "ZERO_RESULTS")
            // Whoops, something else was wrong with the request...
            throw new ApplicationException("There was an error with Google's Geocoding Service: " + status);

        return results;
    }
}