using Client.Model;
using Client.Services;
using IdentityModel.Client;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Client.Pages
{
	public partial class Books
	{
		private List<Book> Catalog = new();
		[Inject] private HttpClient HttpClient { get; set; }
		[Inject] private IConfiguration Config { get; set; }
		[Inject] private ITokenService TokenService { get; set; }

		protected override async Task OnInitializedAsync()
		{
			var tokenResponse = await TokenService.GetToken("Catalog.API.read");
			HttpClient.SetBearerToken(tokenResponse.AccessToken);

			var result = await HttpClient.GetAsync(Config["apiUrl"] + "/api/v1/Book/");

			if (result.IsSuccessStatusCode)
			{
                Catalog = await result.Content.ReadFromJsonAsync<List<Book>>();
			}
		}
	}
}
