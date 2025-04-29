using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TI_Lab3
{
    class FileManager
    {
        public void SaveBytesToFile(SaveFileDialog saveFileDialog, List<int> cipNums)
        {
            try
            {
                saveFileDialog.Filter = "Все файлы (*.*)|*.*";
                saveFileDialog.Title = "Сохранить файл";
                saveFileDialog.RestoreDirectory = true;

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = saveFileDialog.FileName;
                    byte[] bytes = cipNums.Select(i => (byte)i).ToArray();
                    File.WriteAllBytes(filePath, bytes);

                    MessageBox.Show("Файл успешно сохранен!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении файла: {ex.Message}");
            }
        }

        public void SaveToFile(SaveFileDialog saveFileDialog, List<int> cipNums)
        {
            try
            {
                if (cipNums == null || cipNums.Count == 0)
                {
                    MessageBox.Show("Нет данных для сохранения!", "Ошибка",
                                  MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                saveFileDialog.Filter = "Все файлы (*.*)|*.*";
                saveFileDialog.Title = "Сохранить файл";
                saveFileDialog.RestoreDirectory = true;

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = saveFileDialog.FileName;

                    byte[] bytes = new byte[cipNums.Count * sizeof(int)];
                    Buffer.BlockCopy(cipNums.ToArray(), 0, bytes, 0, bytes.Length);
                    File.WriteAllBytes(filePath, bytes);

                    MessageBox.Show("Файл успешно сохранен!", "Успех",
                                  MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении файла: {ex.Message}", "Ошибка",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public List<int> ReadDecFile(OpenFileDialog openFileDialog)
        {
            openFileDialog.Filter = "Все файлы|*.*";
            openFileDialog.Title = "Выберите файл";

            if (openFileDialog.ShowDialog() != DialogResult.OK)
                return new List<int>();

            try
            {
                string filePath = openFileDialog.FileName;
                byte[] bytes = File.ReadAllBytes(filePath);

                if (bytes.Length % sizeof(int) != 0)
                {
                    MessageBox.Show(
                        "Файл повреждён или имеет неверный формат!",
                        "Ошибка",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                    return new List<int>();
                }

                int[] intArray = new int[bytes.Length / sizeof(int)];
                Buffer.BlockCopy(bytes, 0, intArray, 0, bytes.Length);

                return intArray.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при чтении файла: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new List<int>();
            }
        }

        public List<int> ReadBytesFromFile(OpenFileDialog openFileDialog)
        {

            try
            {
                List<int> cipNums = new List<int>();

                openFileDialog.Filter = "Все файлы (*.*)|*.*";
                openFileDialog.Title = "Открыть файл";
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    byte[] bytes = File.ReadAllBytes(filePath);

                    foreach (byte b in bytes)
                        cipNums.Add((int)b);

                    return cipNums;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при чтении файла: {ex.Message}");
            }
            return new List<int>();
        }
    }
}
