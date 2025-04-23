using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace DataTier
{
    public static class ВсеПреподаватели
    {
        public static List<Преподаватель> ПолучитьВсеПреподавателиИзФайла()
        {
            List<Преподаватель> list = new List<Преподаватель>();
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                string json = File.ReadAllText(filePath);
                list = JsonSerializer.Deserialize<List<Преподаватель>>(json);
            }
            return list ?? new List<Преподаватель>();
        }

        public static void СохранитьПреподавателейВФайл(List<Преподаватель> преподаватели, string filePath)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
            string json = JsonSerializer.Serialize(преподаватели, options);
            File.WriteAllText(filePath, json);
        }
    }
}