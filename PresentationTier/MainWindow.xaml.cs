using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DataTier;
using LogicTier;
using Microsoft.Win32;

namespace PresentationTier;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>

public partial class MainWindow : Window
{
    private Университет университет;

    public MainWindow()
    {
        InitializeComponent();
    }

    private void btn_open_file_Click(object sender, RoutedEventArgs e)
    {
        List<Преподаватель> преподавателиИзФайла = ВсеПреподаватели.ПолучитьВсеПреподавателиИзФайла();

        if (преподавателиИзФайла.Count == 0)
        {
            MessageBox.Show("Нет преподавателей в файле!");
            return;
        }

        List<ПреподавательПозиция> позиции = преподавателиИзФайла
            .Select(p => new ПреподавательПозиция(p))
            .ToList();

        университет = new Университет(позиции);
        this.DataContext = университет;
    }

    private void btn_add_to_file_Click(object sender, RoutedEventArgs e)
    {
        SaveFileDialog saveFileDialog = new SaveFileDialog();
        saveFileDialog.Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*";
        saveFileDialog.Title = "Выберите файл для сохранения";

        if (saveFileDialog.ShowDialog() != true) return;

        string ФИО = FIO.Text;
        string Должность = myComboBox.Text;
        string Кафедра = Kaf.Text;
        string ЗП = ZP.Text;

        // Валидация данных
        if (string.IsNullOrWhiteSpace(ФИО) ||
            string.IsNullOrWhiteSpace(Должность) ||
            string.IsNullOrWhiteSpace(Кафедра) ||
            string.IsNullOrWhiteSpace(ЗП))
        {
            MessageBox.Show("Пожалуйста, заполните все поля.");
            return;
        }

        if (!decimal.TryParse(ЗП, out decimal salary))
        {
            MessageBox.Show("Введите корректное числовое значение для зарплаты.");
            return;
        }

        if (salary < 0)
        {
            MessageBox.Show("Зарплата не может быть отрицательной.");
            return;
        }

        // Создаем нового преподавателя
        var новыйПреподаватель = new Преподаватель
        {
            ФИО = ФИО,
            Должность = Должность,
            Кафедра = Кафедра,
            Зарплата = salary
        };

        // Получаем текущий список преподавателей
        var текущиеПреподаватели = университет?.СписокПреподавателей
            .Select(p => new Преподаватель
            {
                ФИО = p.ФИО,
                Должность = p.Должность,
                Кафедра = p.Кафедра,
                Зарплата = p.Зарплата
            })
            .ToList() ?? new List<Преподаватель>();

        // Добавляем нового преподавателя
        текущиеПреподаватели.Add(новыйПреподаватель);

        // Сохраняем в файл
        ВсеПреподаватели.СохранитьПреподавателейВФайл(текущиеПреподаватели, saveFileDialog.FileName);

        MessageBox.Show("Данные успешно сохранены в JSON файл!");

        // Очищаем поля ввода
        FIO.Clear();
        myComboBox.SelectedIndex = -1;
        Kaf.Clear();
        ZP.Clear();
    }

    private void btn_delete_file_Click(object sender, RoutedEventArgs e)
    {
        // Проверка выделенных элементов
        if (MainList.SelectedItems.Count == 0)
        {
            MessageBox.Show("Пожалуйста, выделите элементы для удаления.",
                           "Предупреждение",
                           MessageBoxButton.OK,
                           MessageBoxImage.Warning);
            return;
        }

        // Подтверждение удаления
        var confirmResult = MessageBox.Show(
            $"Вы действительно хотите удалить {MainList.SelectedItems.Count} преподавателей?\n\n" +
            "Это действие нельзя отменить.",
            "Подтверждение удаления",
            MessageBoxButton.YesNo,
            MessageBoxImage.Warning,
            MessageBoxResult.No);

        if (confirmResult != MessageBoxResult.Yes)
        {
            MessageBox.Show("Удаление отменено.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        // Получаем данные и удаляем элементы
        var универ = (Университет)DataContext;
        var itemsToRemove = MainList.SelectedItems.Cast<ПреподавательПозиция>().ToList();

        try
        {
            // Удаление с прогрессом (опционально)
            int removedCount = 0;
            foreach (var item in itemsToRemove)
            {
                универ.СписокПреподавателей.Remove(item);
                removedCount++;
            }

            // Предложение сохранить изменения
            var saveDialogResult = MessageBox.Show(
                $"Успешно удалено {removedCount} преподавателей.\n\nХотите сохранить изменения?",
                "Удаление завершено",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (saveDialogResult == MessageBoxResult.Yes)
            {
                SaveFileDialog sfd = new SaveFileDialog
                {
                    Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*",
                    Title = "Сохранить изменения",
                    FileName = "Преподаватели_обновленные.json"
                };

                if (sfd.ShowDialog() == true)
                {
                    var преподавателиДляСохранения = универ.СписокПреподавателей
                        .Select(p => new Преподаватель
                        {
                            ФИО = p.ФИО,
                            Должность = p.Должность,
                            Кафедра = p.Кафедра,
                            Зарплата = p.Зарплата
                        })
                        .ToList();

                    ВсеПреподаватели.СохранитьПреподавателейВФайл(преподавателиДляСохранения, sfd.FileName);
                    MessageBox.Show($"Данные сохранены в файл:\n{sfd.FileName}",
                                  "Сохранено",
                                  MessageBoxButton.OK,
                                  MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Изменения не сохранены.",
                                   "Информация",
                                   MessageBoxButton.OK,
                                   MessageBoxImage.Information);
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка при удалении: {ex.Message}",
                          "Ошибка",
                          MessageBoxButton.OK,
                          MessageBoxImage.Error);
        }
    }
    private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (myComboBox.SelectedItem != null)
        {
            // Получаем выбранный элемент
            var selectedItem = (ComboBoxItem)myComboBox.SelectedItem;
        }
    }
}




