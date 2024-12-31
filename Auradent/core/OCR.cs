using System;
using System.Threading.Tasks;
using Tesseract;
using System.IO;

namespace Auradent.core
{
    public static class OCR
    {
        private const string TESSDATA_PATH = "tessdata";

        public static async Task<string> GetText(byte[] imageBytes)
        {
            try
            {
                using (var engine = new TesseractEngine(TESSDATA_PATH, "eng", EngineMode.Default))
                {
                    using (var ms = new MemoryStream(imageBytes))
                    {
                        using (var img = Pix.LoadFromMemory(ms.ToArray()))
                        {
                            using (var page = engine.Process(img))
                            {
                                return page.GetText();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"OCR processing failed: {ex.Message}");
            }
        }
    }
}