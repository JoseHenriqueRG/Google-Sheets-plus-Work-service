using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Jobs.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;

namespace Jobs.Google.Drive
{
    public class PlanilhaClassificacaoTecnica : ISpreadsheet
    {
        private readonly string[] Scopes = { SheetsService.Scope.Spreadsheets };
        private readonly string _applicationName;
        private readonly string _spreadsheetId;
        private readonly string _sheet;
        private SheetsService service;

        public PlanilhaClassificacaoTecnica(IConfiguration configuration)
        {
            _applicationName = configuration["ClassificacaoTecnica:ApplicationName"].ToString();
            _spreadsheetId = configuration["ClassificacaoTecnica:Id"].ToString();
            _sheet = configuration["ClassificacaoTecnica:SheetName"].ToString();
        }

        public void CreateService()
        {
            GoogleCredential credential;
            using (var stream = new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream)
                    .CreateScoped(Scopes);
            }

            // Create Google Sheets API service.
            service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = _applicationName,
            });

            DeleteData();
            UpdateData();
        }
        
        public void CreateData()
        {
            ClassificacaoTecnica classificacaoTecnica = new();

            var lista = classificacaoTecnica.List();

            var rangeAux = lista.Count + 1;
            var range = $"{_sheet}!A1:H{rangeAux}";
            var valueRange = new ValueRange();

            var oblist = new List<object> { "ID", "Coloração", "Corte", "Setor", "Usuario", "Observação", "Data Lâmina" , "Data de Avaliação"};
            valueRange.Values = new List<IList<object>> { oblist };

            foreach (var item in lista)
            {
                oblist = new List<object>() { item.ID, item.Coloracao, item.Corte, item.Setor, item.Usuario, item.Observacao, item.DataLamina, item.DataAvaliacao };
                valueRange.Values.Add(oblist);
            }

            var appendRequest = service.Spreadsheets.Values.Append(valueRange, _spreadsheetId, range);
            appendRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.USERENTERED;
            var appendReponse = appendRequest.Execute();
        }

        public void UpdateData()
        {
            var lista = new ClassificacaoTecnica().List();

            var rangeAux = lista.Count + 1;
            var range = $"{_sheet}!A1:H{rangeAux}";
            var valueRange = new ValueRange();

            var oblist = new List<object> { "ID", "Coloração", "Corte", "Setor", "Usuario", "Observação", "Data Lâmina", "Data de Avaliação" };
            valueRange.Values = new List<IList<object>> { oblist };

            foreach (var item in lista)
            {
                oblist = new List<object>() { item.ID, item.Coloracao, item.Corte, item.Setor, item.Usuario, item.Observacao, item.DataLamina, item.DataAvaliacao };
                valueRange.Values.Add(oblist);
            }

            var updateRequest = service.Spreadsheets.Values.Update(valueRange, _spreadsheetId, range);
            updateRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;
            var appendReponse = updateRequest.Execute();
        }

        public void DeleteData()
        {
            var range = $"{_sheet}!A:Z";
            var requestBody = new ClearValuesRequest();

            var deleteRequest = service.Spreadsheets.Values.Clear(requestBody, _spreadsheetId, range);
            var deleteReponse = deleteRequest.Execute();
        }

    }
}
