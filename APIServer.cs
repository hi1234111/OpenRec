using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using api;
using api2018;
using api2017;
using Newtonsoft.Json;
using vaultgamesesh;
using System.Collections.Generic;
namespace Server
{
	// Token: 0x0200000C RID: 12
	internal class APIServer
	{
		// Token: 0x06000090 RID: 144 RVA: 0x00005628 File Offset: 0x00003828
		public APIServer()
		{
			try
			{
				Console.WriteLine("[API] Server Started!");
				APIServer.listen.Start();
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06000091 RID: 145 RVA: 0x00005664 File Offset: 0x00003864
		public static void ADListen()
		{
			APIServer.server.Prefixes.Add("http://localhost:2000/");
			try
			{
				for (;;)
				{
					APIServer.server.Start();
					Console.WriteLine("[API] Listening");
					HttpListenerContext context = APIServer.server.GetContext();
					string rawurl = context.Request.RawUrl;
					Console.WriteLine("[API] Requested! URL: " + rawurl + ".");
					string post;
					using (StreamReader getpost = new StreamReader(context.Request.InputStream))
					{
						post = getpost.ReadToEnd();
					}
					string r;
					if (rawurl.StartsWith("/api/versioncheck/"))
					{
						r = "{\"ValidVersion\":true}";
					}
					else if (rawurl.StartsWith("/api/versioncheck/"))
					{
						r = "{\"ValidVersion\":true}";
					}
					else if (rawurl.StartsWith("/api/config/v2"))
					{
						r = Config.GetConfig(false);
					}
					else if (rawurl.StartsWith("/api/platformlogin/v1/getcachedlogins"))
					{
						r = Login.GCL();
					}
					else if (rawurl.StartsWith("/api/platformlogin/v1/logincached"))
					{
						r = Login.CLogin();
					}
					else if (rawurl.StartsWith("/api/PlayerReporting/v1/moderationBlockDetails"))
					{
						r = "{\"ReportCatagory\":0,\"Duration\":0,\"GameSessionId\":0,\"Message\":\"\"}";
					}
					else if (rawurl.StartsWith("/api/config/v1/amplitude"))
					{
						r = "{\"AmplitudeKey\":\"RecNoot\"}";
					}
					else if (rawurl.StartsWith("/api/images/v2/named"))
					{
						r = "[{\"FriendlyImageName\":\"DormRoomBucket\",\"ImageName\":\"n/67gesMhbOkCd3-qO1cKkIg\",\"StartTime\":\"2018-09-27T18:00:00Z\",\"EndTime\":\"2222-02-22T22:22:00Z\"}]";
					}
					else if (rawurl.StartsWith("/api/avatar/v2/gifts/generate"))
					{
						r = Storefront.GenerateGift();
					}
					else if (rawurl.StartsWith("/api/avatar/v2/gifts/consume"))
					{
						Storefront.DisposeAndAwardGift();
						r = "[]";
					}
					else if (rawurl.StartsWith("/api/avatar/v2/gifts"))
					{
						r = "[]";
					}
					else if (rawurl.StartsWith("/api/avatar/v3/items"))
					{
						r = File.ReadAllText(Program.DataPath + "\\AVItems.txt");
					}
					else if (rawurl.StartsWith("/api/avatar/v2/set"))
					{
						AvatarAPI.SaveAvatar(post);
						r = "i'm saved dummy";
					}
					else if (rawurl.StartsWith("/api/avatar/v2"))
					{
						r = File.ReadAllText(Program.ProfilePath + "\\avatar.txt");
					}
					else if (rawurl.StartsWith("/api/settings/v2/set"))
					{
						Settings.SaveSettings(post);
						r = "seted deez nutz";
					}
					else if (rawurl.StartsWith("/api/settings/v2"))
					{
						r = File.ReadAllText(Program.ProfilePath + "\\settings.txt");
					}
					else if (rawurl.StartsWith("/api/objectives/v1/myprogress"))
					{
						r = File.ReadAllText(Program.DataPath + "\\progress.txt");
					}
					else if (rawurl.StartsWith("/api/playerevents/v1/all"))
					{
						r = "{\"Created\":[],\"Responses\":[]}";
					}
					else if (rawurl.StartsWith("/api/gamesessions/v2/joinrandom"))
					{
						r = GameSessions.V2JoinRandom(post);
					}
					else if (rawurl.StartsWith("/api/gamesessions/v3/joinroom"))
					{
						r = GameSessionsV3.joinRoom(post);
					}
					else if (rawurl.StartsWith("/api/gameconfigs/v1/all"))
					{
						r = File.ReadAllText(Program.DataPath + "\\GC.txt");
					}
					else if (rawurl.StartsWith("/api/presence/v3/heartbeat"))
					{
						Console.WriteLine(post);
						r = JsonConvert.SerializeObject(GameSessionsV3.PresenceV3());
					}
					else if (rawurl.StartsWith("/api/rooms/v1/hot"))
					{
						GameSessionsV3.GetMenuRooms();
						r = JsonConvert.SerializeObject(GameSessionsV3.MenuRoom);
					}
					else if (rawurl.StartsWith("/api/rooms/v4/details/"))
					{
						string text = rawurl.Substring(22);
						Console.Write(text);
						r = GameSessionsV3.GetDetails(text);
					}
					else if (rawurl.StartsWith("/api/storefronts/v3/balance/"))
					{
						r = Storefront.GetBalence(int.Parse(rawurl.Substring(rawurl.Length - 1)));
					}
					else if (rawurl.StartsWith("/api/images/v1/slideshow"))
					{
						r = Login.notavirus.DownloadString("https://coffeeman240.github.io/CoffeeVaultRBSData/Slideshow.json");
					}
					else if (rawurl.StartsWith("//api/sanitize/v1/isPure"))
					{
						r = "{\"IsPure\":true}";
					}
					else if (rawurl.StartsWith("/api/objectives/v1/myprogress"))
					{
						r = Login.notavirus.DownloadString("https://coffeeman240.github.io/CoffeeVaultRBSData/Challenges/objectives.json");
					}
					else if (rawurl.StartsWith("/api/challenge/v1/getCurrent"))
					{
						r = "{\"Success\":true,\"Message\":\"\"," + Login.notavirus.DownloadString("https://coffeeman240.github.io/CoffeeVaultRBSData/Challenges/challenges.json") + "}";
					}
					else if (rawurl.StartsWith("/api/checklist/v1/current"))
					{
						r = Login.notavirus.DownloadString("https://coffeeman240.github.io/CoffeeVaultRBSData/Challenges/checklist.json");
					}
					else
					{
						r = "[]";
					}
					byte[] bytes = Encoding.UTF8.GetBytes(r);
					context.Response.ContentLength64 = (long)bytes.Length;
					context.Response.OutputStream.Write(bytes, 0, bytes.Length);
					Thread.Sleep(10);
					APIServer.server.Stop();
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("[API] CRASH! The Application Will Now Exit.");
				File.WriteAllText(Environment.CurrentDirectory + "\\crashlog.txt", ex.ToString());
				Thread.Sleep(5000);
				Environment.Exit(0);
			}
		}

		// Token: 0x06000092 RID: 146 RVA: 0x000024D2 File Offset: 0x000006D2
		// Note: this type is marked as 'beforefieldinit'.
		static APIServer()
		{
		}

		// Token: 0x04000044 RID: 68
		public static HttpListener server = new HttpListener();

		// Token: 0x04000045 RID: 69
		public static Thread listen = new Thread(new ThreadStart(APIServer.ADListen));

		// Token: 0x04000046 RID: 70
		public static Dictionary<string, string> missingApis;
	}
}
