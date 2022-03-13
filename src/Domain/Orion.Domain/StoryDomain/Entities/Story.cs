using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Orion.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Domain.StoryDomain.Entities
{
    public class Story : Entity
    {
        [JsonProperty("text")]
        public string Text { get; private set; }
        [JsonProperty("images")]
        public string[] Images { get; private set; }
        private Story()
        {
            Text = String.Empty;
        }
        public static Story Create(string text, string[]? images=null)
        {
            var story = new Story();
            story.Text = story.DoesTextContainsBadWords(text) ? throw new BusinessRuleValidationException(ErrorMessages.DetectedBadWordsInText) : text;
            story.Images = images;
            story.CreatedAt = SystemClock.Now;
            return story;
        }

        public void UpdateText(string text)
        {
            Text = DoesTextContainsBadWords(text) ? throw new BusinessRuleValidationException(ErrorMessages.DetectedBadWordsInText) : text;
        }

        private bool DoesTextContainsBadWords(string text)
        {
            //ToDO: use some third party api to detect bad words in text
            var badWords = new string[] { "badword1", "badword2", "badword3" };

            var textToLower = text.ToLower();

            foreach (var badWord in badWords)
            {
                if (textToLower.Contains(badWord))
                {
                    return true;
                }
            }

            return false;
        }
    }
}