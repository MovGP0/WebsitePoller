using System.Text;
using RestSharp;
using RestSharp.Extensions.MonoHttp;
using WebsitePoller.Setting;

namespace WebsitePoller.FormRegistrator
{
    public static class PostalAddressExtensions
    {
        public static string BuildAjaxQuery(this PostalAddress address)
        {
            return new StringBuilder()
                .Append("<xjxquery><q>")
                .AppendParameter("Anrede", address.Salutation, true)
                .AppendParameter("Familienname", address.FamilyName)
                .AppendParameter("Vorname", address.FirstName)
                .AppendParameter("Titel", address.Title)
                .AppendParameter("GebdatTT", address.BirthDate.Day.ToString())
                .AppendParameter("GebdatMM", address.BirthDate.Month.ToString())
                .AppendParameter("GebdatYY", address.BirthDate.Year.ToString())
                .AppendParameter("Adresse", address.Street)
                .AppendParameter("Hausnummer", address.HouseNumber)
                .AppendParameter("Stiege", address.StairsNumber)
                .AppendParameter("Tuer", address.ApartmentNumber)
                .AppendParameter("Postleitzahl", address.PostalCode.ToString())
                .AppendParameter("Ort", address.City)
                .AppendParameter("Telefon", address.PhoneNumber)
                .AppendParameter("Email", address.EmailAddress)
                .AppendParameter("Mieter", address.IsTenant ? "Ja" : "Nein")
                .Append("&submit=Senden")
                .Append("</q></xjxquery>")
                .ToString();
        }

        public static RestRequest BuildContactRequest(this PostalAddress postalAddress, string href)
        {
            var contactRequest = new RestRequest(href, Method.POST);
            contactRequest.AddHeader("Content-Type", "application/x-www-form-urlencoded");

            var queryParameters = HttpUtility.ParseQueryString(HttpUtility.UrlDecode(href));
            var mobjnr = queryParameters["tx_sozaltbau_pi1[mobjnr]"];
            var mlfd = queryParameters["tx_sozaltbau_pi1[mlfd]"];

            contactRequest.AddQueryParameter("xajax", "processMailForm");
            contactRequest.AddQueryParameter("xajaxargs[]", postalAddress.BuildAjaxQuery());
            contactRequest.AddQueryParameter("xajaxargs[]", mobjnr);
            contactRequest.AddQueryParameter("xajaxargs[]", mlfd);

            return contactRequest;
        }
    }
}