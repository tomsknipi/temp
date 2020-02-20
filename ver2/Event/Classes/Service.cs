using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using MobileLibrary.Extension;
using Newtonsoft.Json;
using TomskNipi.Event.MobileClasses;
using TomskNipi.Event.MobileClasses.Enum;
using Xamarin.Essentials;

namespace Event.Classes
{
    /// <summary>
    /// Связь с сервером
    /// </summary>
    public static class Service
    {
        private static readonly TimeSpan Timeout = new TimeSpan(0, 0, 10);
        private static readonly HttpClient Client = new HttpClient {Timeout = Timeout};
        private static readonly string Connection =
#if DEBUG
            @"http://pc-5202.tomsknipi.ru:52465/api/Mobile/";
#else
            @"http://mobile.tomsknipi.ru/event/api/Mobile/";
#endif

        static Service()
        {
            Client.DefaultRequestHeaders.TryAddWithoutValidation(MobConst.HeaderTestPlatform, DeviceInfo.Platform.ToString());
            Client.DefaultRequestHeaders.TryAddWithoutValidation(MobConst.HeaderTestAppBuild, Config.AppBuild);
        }

        #region Методы

        private static async Task<MobResult<string>> ExecuteGet(string query, string method)
        {
            try
            {
                var url = ExecuteUrl(query, method);
                try
                {
                    await Client.GetAsync(ExecuteUrl(string.Empty, "TestLive"));
                }
                catch
                {
                    // ignored
                }

                var response = await Client.GetAsync(url);
                ExecuteValidateResponse(response);

                return new MobResult<string> {Data = await response.Content.ReadAsStringAsync()};
            }
            catch (OperationCanceledException e)
            {
                return new MobResult<string> {Error = true, Text = "Не удается получить доступ к сервису!\nВозможно отсутствует подключение к Интернет или сервис не доступен.", ErrorStack = e.StackTrace};
            }
            catch (Exception e)
            {
                return new MobResult<string> {Error = true, Text = e.Message, ErrorStack = e.StackTrace};
            }
        }

        private static async Task<MobResult<string>> ExecutePost(string query, string method, string stringContent)
        {
            try
            {
                var url = ExecuteUrl(query, method);
                try
                {
                    await Client.GetAsync(ExecuteUrl(string.Empty, "TestLive"));
                }
                catch
                {
                    // ignored
                }

                var content = new StringContent(stringContent);
                var response = await Client.PostAsync(url, content);
                ExecuteValidateResponse(response);

                return new MobResult<string> {Data = await response.Content.ReadAsStringAsync()};
            }
            catch (OperationCanceledException e)
            {
                return new MobResult<string> {Error = true, Text = "Не удается получить доступ к сервису!\nВозможно отсутствует подключение к Интернет или сервис не доступен.", ErrorStack = e.StackTrace};
            }
            catch (Exception e)
            {
                return new MobResult<string> {Error = true, Text = e.Message, ErrorStack = e.StackTrace};
            }
        }

        private static string ExecuteUrl(string query, string method)
        {
            var builder = new UriBuilder(Connection + method) {Query = query};

            return builder.ToString();
        }

        private static void ExecuteValidateResponse(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
                return;
            var mes = response.ReasonPhrase.IsNullOrEmpty() ? string.Empty : response.ReasonPhrase + "\n";
            mes += $"Код ошибки: {(int) response.StatusCode}";
            throw new Exception(mes);
        }

        /// <summary>
        /// Аутентификация пользователя
        /// </summary>
        public static async Task<MobResult<MobUser>> GetAuthentication(string login, string password)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["guid"] = MobConst.Guid;
            query[nameof(login)] = login;
            query[nameof(password)] = password;
            var result = await ExecuteGet(query.ToString(), nameof(GetAuthentication));

            return result.Error
                ? new MobResult<MobUser> {Error = true, Text = result.Text, ErrorStack = result.ErrorStack}
                : JsonConvert.DeserializeObject<MobResult<MobUser>>(result.Data);
        }

        /// <summary>
        /// Список комментариев
        /// </summary>
        public static async Task<MobResult<List<MobComment>>> GetCommentList(Guid eventId, string typeName, Guid typeId)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["guid"] = MobConst.Guid;
            query[nameof(eventId)] = eventId.AsString();
            query[nameof(typeName)] = typeName.AsString();
            query[nameof(typeId)] = typeId.AsString();
            var result = await ExecuteGet(query.ToString(), nameof(GetCommentList));

            return result.Error
                ? new MobResult<List<MobComment>> {Error = true, Text = result.Text, ErrorStack = result.ErrorStack}
                : JsonConvert.DeserializeObject<MobResult<List<MobComment>>>(result.Data);
        }

        /// <summary>
        /// Мероприятие
        /// </summary>
        public static async Task<MobResult<MobEvent>> GetEvent(Guid userId, Guid eventId, bool getAny)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["guid"] = MobConst.Guid;
            query[nameof(userId)] = userId.AsString();
            query[nameof(eventId)] = eventId.AsString();
            query[nameof(getAny)] = getAny.AsString();
            var result = await ExecuteGet(query.ToString(), nameof(GetEvent));

            return result.Error
                ? new MobResult<MobEvent> {Error = true, Text = result.Text, ErrorStack = result.ErrorStack}
                : JsonConvert.DeserializeObject<MobResult<MobEvent>>(result.Data);
        }

        /// <summary>
        /// Список мерориятий
        /// </summary>
        public static async Task<MobResult<List<MobEventMenu>>> GetEventMenuList(Guid userId, bool nextPage, DateTime lastDateIn)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["guid"] = MobConst.Guid;
            query[nameof(userId)] = userId.AsString();
            query[nameof(nextPage)] = nextPage.AsString();
            query[nameof(lastDateIn)] = lastDateIn.ToString("yyyy-MM-ddTHH:mm:ss");
            var result = await ExecuteGet(query.ToString(), nameof(GetEventMenuList));

            return result.Error
                ? new MobResult<List<MobEventMenu>> {Error = true, Text = result.Text, ErrorStack = result.ErrorStack}
                : JsonConvert.DeserializeObject<MobResult<List<MobEventMenu>>>(result.Data);
        }

        /// <summary>
        /// Лайк
        /// </summary>
        public static async Task<MobResult<MobLike>> GetLike(Guid userId, Guid eventId, string typeName, Guid typeId)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["guid"] = MobConst.Guid;
            query[nameof(userId)] = userId.AsString();
            query[nameof(eventId)] = eventId.AsString();
            query[nameof(typeName)] = typeName.AsString();
            query[nameof(typeId)] = typeId.AsString();
            var result = await ExecuteGet(query.ToString(), nameof(GetLike));

            return result.Error
                ? new MobResult<MobLike> {Error = true, Text = result.Text, ErrorStack = result.ErrorStack}
                : JsonConvert.DeserializeObject<MobResult<MobLike>>(result.Data);
        }

        /// <summary>
        /// Список новостей
        /// </summary>
        public static async Task<MobResult<List<MobNew>>> GetNewList(Guid userId, Guid eventId)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["guid"] = MobConst.Guid;
            query[nameof(userId)] = userId.AsString();
            query[nameof(eventId)] = eventId.AsString();
            var result = await ExecuteGet(query.ToString(), nameof(GetNewList));

            return result.Error
                ? new MobResult<List<MobNew>> {Error = true, Text = result.Text, ErrorStack = result.ErrorStack}
                : JsonConvert.DeserializeObject<MobResult<List<MobNew>>>(result.Data);
        }

        /// <summary>
        /// Возвращает ссылку на фотографию новости
        /// </summary>
        public static string GetNewPhoto(Guid newId)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["guid"] = MobConst.Guid;
            query[nameof(newId)] = newId.AsString();

            return ExecuteUrl(query.ToString(), nameof(GetNewPhoto));
        }

        /// <summary>
        /// Возвращает ссылку на файл презентационных материалов
        /// </summary>
        public static string GetPresentationFile(Guid presentationId, string fileName)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query[nameof(presentationId)] = presentationId.AsString();
            query[nameof(fileName)] = fileName.AsString();

            return ExecuteUrl(query.ToString(), nameof(GetPresentationFile));
        }

        /// <summary>
        /// Список презентационных материалов
        /// </summary>
        public static async Task<MobResult<List<MobPresentation>>> GetPresentationList(Guid eventId)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["guid"] = MobConst.Guid;
            query[nameof(eventId)] = eventId.AsString();
            var result = await ExecuteGet(query.ToString(), nameof(GetPresentationList));

            return result.Error
                ? new MobResult<List<MobPresentation>> {Error = true, Text = result.Text, ErrorStack = result.ErrorStack}
                : JsonConvert.DeserializeObject<MobResult<List<MobPresentation>>>(result.Data);
        }

        /// <summary>
        /// Список секций
        /// </summary>
        public static async Task<MobResult<List<MobSection>>> GetSectionList(Guid eventId)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["guid"] = MobConst.Guid;
            query[nameof(eventId)] = eventId.AsString();
            var result = await ExecuteGet(query.ToString(), nameof(GetSectionList));

            return result.Error
                ? new MobResult<List<MobSection>> {Error = true, Text = result.Text, ErrorStack = result.ErrorStack}
                : JsonConvert.DeserializeObject<MobResult<List<MobSection>>>(result.Data);
        }

        /// <summary>
        /// Список докладов
        /// </summary>
        public static async Task<MobResult<List<MobSectionReport>>> GetSectionReportList(Guid sectionId)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["guid"] = MobConst.Guid;
            query[nameof(sectionId)] = sectionId.AsString();
            var result = await ExecuteGet(query.ToString(), nameof(GetSectionReportList));

            return result.Error
                ? new MobResult<List<MobSectionReport>> {Error = true, Text = result.Text, ErrorStack = result.ErrorStack}
                : JsonConvert.DeserializeObject<MobResult<List<MobSectionReport>>>(result.Data);
        }

        /// <summary>
        /// Расписание
        /// </summary>
        public static async Task<MobResult<List<MobShedule>>> GetSheduleList(Guid eventId)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["guid"] = MobConst.Guid;
            query[nameof(eventId)] = eventId.AsString();
            var result = await ExecuteGet(query.ToString(), nameof(GetSheduleList));

            return result.Error
                ? new MobResult<List<MobShedule>> {Error = true, Text = result.Text, ErrorStack = result.ErrorStack}
                : JsonConvert.DeserializeObject<MobResult<List<MobShedule>>>(result.Data);
        }

        /// <summary>
        /// Список участников
        /// </summary>
        public static async Task<MobResult<List<MobUser>>> GetUserList(Guid eventId, bool nextPage, string lastName)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["guid"] = MobConst.Guid;
            query[nameof(eventId)] = eventId.AsString();
            query[nameof(nextPage)] = nextPage.AsString();
            query[nameof(lastName)] = lastName.AsString();
            var result = await ExecuteGet(query.ToString(), nameof(GetUserList));

            return result.Error
                ? new MobResult<List<MobUser>> {Error = true, Text = result.Text, ErrorStack = result.ErrorStack}
                : JsonConvert.DeserializeObject<MobResult<List<MobUser>>>(result.Data);
        }

        /// <summary>
        /// Возвращает ссылку на фотографию пользователя
        /// </summary>
        public static string GetUserPhoto(Guid userId)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["guid"] = MobConst.Guid;
            query[nameof(userId)] = userId.AsString();

            return ExecuteUrl(query.ToString(), nameof(GetUserPhoto));
        }

        /// <summary>
        /// Сохранение комментария
        /// </summary>
        /// <returns></returns>
        public static async Task<MobResult<MobComment>> PostComment(Guid userId, Guid eventId, string typeName, Guid typeId, string commentValue)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["guid"] = MobConst.Guid;
            query[nameof(userId)] = userId.AsString();
            query[nameof(eventId)] = eventId.AsString();
            query[nameof(typeName)] = typeName.AsString();
            query[nameof(typeId)] = typeId.AsString();
            query[nameof(commentValue)] = commentValue.AsString();
            var result = await ExecutePost(query.ToString(), nameof(PostComment), string.Empty);

            return result.Error
                ? new MobResult<MobComment> {Error = true, Text = result.Text, ErrorStack = result.ErrorStack}
                : JsonConvert.DeserializeObject<MobResult<MobComment>>(result.Data);
        }

        /// <summary>
        /// Сохранение лайка
        /// </summary>
        public static async Task<MobResult<MobLike>> PostLike(Guid userId, Guid eventId, string typeName, Guid typeId, LikeType likeValue)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["guid"] = MobConst.Guid;
            query[nameof(userId)] = userId.AsString();
            query[nameof(eventId)] = eventId.AsString();
            query[nameof(typeName)] = typeName.AsString();
            query[nameof(typeId)] = typeId.AsString();
            query[nameof(likeValue)] = likeValue.AsString();
            var result = await ExecutePost(query.ToString(), nameof(PostLike), string.Empty);

            return result.Error
                ? new MobResult<MobLike> {Error = true, Text = result.Text, ErrorStack = result.ErrorStack}
                : JsonConvert.DeserializeObject<MobResult<MobLike>>(result.Data);
        }

        /// <summary>
        /// Устанавливает push-уведомления как прочитанные
        /// </summary>
        public static async Task<MobResult<bool>> PostMessageDateRead(Guid userId, Guid eventId, MessageType messageType)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["guid"] = MobConst.Guid;
            query[nameof(userId)] = userId.AsString();
            query[nameof(eventId)] = eventId.AsString();
            query[nameof(messageType)] = messageType.AsString();
            var result = await ExecutePost(query.ToString(), nameof(PostMessageDateRead), string.Empty);

            return result.Error
                ? new MobResult<bool> {Error = true, Text = result.Text, ErrorStack = result.ErrorStack}
                : JsonConvert.DeserializeObject<MobResult<bool>>(result.Data);
        }

        /// <summary>
        /// Сохранение токена пользователя
        /// </summary>
        public static async Task<MobResult<bool>> PostToken(Guid userId, string tokenValue, string deviceIdent, string deviceName, string platform, string appVersion)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["guid"] = MobConst.Guid;
            query[nameof(userId)] = userId.AsString();
            query[nameof(tokenValue)] = tokenValue.AsString();
            query[nameof(deviceIdent)] = deviceIdent.AsString();
            query[nameof(deviceName)] = deviceName.AsString();
            query[nameof(platform)] = platform.AsString();
            query[nameof(appVersion)] = appVersion.AsString();
            var result = await ExecutePost(query.ToString(), nameof(PostToken), string.Empty);

            return result.Error
                ? new MobResult<bool> {Error = true, Text = result.Text, ErrorStack = result.ErrorStack}
                : JsonConvert.DeserializeObject<MobResult<bool>>(result.Data);
        }

        /// <summary>
        /// Изменение пароля пользователя
        /// </summary>
        public static async Task<MobResult<bool>> PostUserPassword(Guid userId, string lastPassword, string newPassword)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["guid"] = MobConst.Guid;
            query[nameof(userId)] = userId.AsString();
            query[nameof(lastPassword)] = lastPassword.AsString();
            query[nameof(newPassword)] = newPassword.AsString();
            var result = await ExecutePost(query.ToString(), nameof(PostUserPassword), string.Empty);

            return result.Error
                ? new MobResult<bool> {Error = true, Text = result.Text, ErrorStack = result.ErrorStack}
                : JsonConvert.DeserializeObject<MobResult<bool>>(result.Data);
        }

        /// <summary>
        /// Сохранение фотографии пользователя
        /// </summary>
        public static async Task<MobResult<bool>> PostUserPhoto(Guid id, byte[] photo)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["guid"] = MobConst.Guid;
            var result = await ExecutePost(query.ToString(), nameof(PostUserPhoto), JsonConvert.SerializeObject(new MobUserPhoto(id, photo)));

            return result.Error
                ? new MobResult<bool> {Error = true, Text = result.Text, ErrorStack = result.ErrorStack}
                : JsonConvert.DeserializeObject<MobResult<bool>>(result.Data);
        }

        #endregion
    }
}