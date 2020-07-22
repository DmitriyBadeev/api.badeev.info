using NUnit.Framework;

namespace Portfolio.Typograf.Test
{
    public class Tests
    {
        [Test]
        public void Common1()
        {
            var inputText = "Это набор базовых установок и принципов, которые являются основой моих убеждений и взглядов на жизнь. Используется для облегчения принятия решений, а также для вынесения своих вердиктов по поводу любых событий.";
            var outputText = "Это набор базовых установок и&nbsp;принципов, которые являются основой моих убеждений и&nbsp;взглядов на&nbsp;жизнь. Используется для облегчения принятия решений, а&nbsp;также для вынесения своих вердиктов по&nbsp;поводу любых событий.";

            var typografText = Typograf.Run(inputText).Result;

            Assert.AreEqual(outputText, typografText);
        }

        [Test]
        public void Common2()
        {
            var inputText = "Я против любого насилия, против любых убийств, против любых войн. Всегда ставлю жизнь человека на первое место.";
            var outputText = "Я&nbsp;против любого насилия, против любых убийств, против любых войн. Всегда ставлю жизнь человека на&nbsp;первое место.";

            var typografText = Typograf.Run(inputText).Result;

            Assert.AreEqual(outputText, typografText);
        }

        [Test]
        public void Common3()
        {
            var inputText = "Я всегда уважаю чужое мнение, если оно не противоречит пункту 1. Я поддерживаю критику своих и чужих мнений и мировоззрений, но оставляю себе право на защиту своей позиции. Я против дискриминации и оскорблений человека из-за несовпадающих позиций по какому-то вопросу. Я за критику мнений, но не за критику человека.";
            var outputText = "Я&nbsp;всегда уважаю чужое мнение, если оно не&nbsp;противоречит пункту 1. Я&nbsp;поддерживаю критику своих и&nbsp;чужих мнений и&nbsp;мировоззрений, но&nbsp;оставляю себе право на&nbsp;защиту своей позиции. Я&nbsp;против дискриминации и&nbsp;оскорблений человека из-за несовпадающих позиций по&nbsp;какому-то вопросу. Я&nbsp;за&nbsp;критику мнений, но&nbsp;не&nbsp;за&nbsp;критику человека.";

            var typografText = Typograf.Run(inputText).Result;

            Assert.AreEqual(outputText, typografText);
        }

        [Test]
        public void CheckDash()
        {
            var inputText = "Колян - за пивом!";
            var outputText = "Колян&nbsp;&mdash; за&nbsp;пивом!";

            var typografText = Typograf.Run(inputText).Result;

            Assert.AreEqual(outputText, typografText);
        }

        [Test]
        public void CheckQuotes()
        {
            var inputText = "\"Если вы не занимаетесь политикой, то политика займётся вами\" - Отто фон Бисмарк.";
            var outputText = "&laquo;Если вы&nbsp;не&nbsp;занимаетесь политикой, то&nbsp;политика займётся вами&raquo;&nbsp;&mdash; Отто фон Бисмарк.";

            var typografText = Typograf.Run(inputText).Result;

            Assert.AreEqual(outputText, typografText);
        }

        [Test]
        public void CheckEncoding()
        {
            var inputText = "Это набор базовых установок и&nbsp;принципов, которые являются основой моих убеждений и&nbsp;взглядов на&nbsp;жизнь. Используется для облегчения принятия решений, а&nbsp;также для вынесения своих вердиктов по&nbsp;поводу любых событий.";
            var outputText = "Это набор базовых установок и&nbsp;принципов, которые являются основой моих убеждений и&nbsp;взглядов на&nbsp;жизнь. Используется для облегчения принятия решений, а&nbsp;также для вынесения своих вердиктов по&nbsp;поводу любых событий.";

            var typografText = Typograf.Run(inputText).Result;

            Assert.AreEqual(outputText, typografText);
        }
    }
}