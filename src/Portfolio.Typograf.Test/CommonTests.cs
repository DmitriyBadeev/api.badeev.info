using NUnit.Framework;

namespace Portfolio.Typograf.Test
{
    public class Tests
    {
        [Test]
        public void Common1()
        {
            var inputText = "Ёто набор базовых установок и принципов, которые €вл€ютс€ основой моих убеждений и взгл€дов на жизнь. »спользуетс€ дл€ облегчени€ прин€ти€ решений, а также дл€ вынесени€ своих вердиктов по поводу любых событий.";
            var outputText = "Ёто набор базовых установок и&nbsp;принципов, которые €вл€ютс€ основой моих убеждений и&nbsp;взгл€дов на&nbsp;жизнь. »спользуетс€ дл€ облегчени€ прин€ти€ решений, а&nbsp;также дл€ вынесени€ своих вердиктов по&nbsp;поводу любых событий.";

            var typografText = Typograf.Run(inputText).Result;

            Assert.AreEqual(outputText, typografText);
        }

        [Test]
        public void Common2()
        {
            var inputText = "я против любого насили€, против любых убийств, против любых войн. ¬сегда ставлю жизнь человека на первое место.";
            var outputText = "я&nbsp;против любого насили€, против любых убийств, против любых войн. ¬сегда ставлю жизнь человека на&nbsp;первое место.";

            var typografText = Typograf.Run(inputText).Result;

            Assert.AreEqual(outputText, typografText);
        }

        [Test]
        public void Common3()
        {
            var inputText = "я всегда уважаю чужое мнение, если оно не противоречит пункту 1. я поддерживаю критику своих и чужих мнений и мировоззрений, но оставл€ю себе право на защиту своей позиции. я против дискриминации и оскорблений человека из-за несовпадающих позиций по какому-то вопросу. я за критику мнений, но не за критику человека.";
            var outputText = "я&nbsp;всегда уважаю чужое мнение, если оно не&nbsp;противоречит пункту 1. я&nbsp;поддерживаю критику своих и&nbsp;чужих мнений и&nbsp;мировоззрений, но&nbsp;оставл€ю себе право на&nbsp;защиту своей позиции. я&nbsp;против дискриминации и&nbsp;оскорблений человека из-за несовпадающих позиций по&nbsp;какому-то вопросу. я&nbsp;за&nbsp;критику мнений, но&nbsp;не&nbsp;за&nbsp;критику человека.";

            var typografText = Typograf.Run(inputText).Result;

            Assert.AreEqual(outputText, typografText);
        }

        [Test]
        public void Common4()
        {
            var inputText = " ол€н - за пивом!";
            var outputText = " ол€н&nbsp;&mdash; за&nbsp;пивом!";

            var typografText = Typograf.Run(inputText).Result;

            Assert.AreEqual(outputText, typografText);
        }

        [Test]
        public void Common5()
        {
            var inputText = "\"≈сли вы не занимаетесь политикой, то политика займЄтс€ вами\" - ќтто фон Ѕисмарк.";
            var outputText = "&laquo;≈сли вы&nbsp;не&nbsp;занимаетесь политикой, то&nbsp;политика займЄтс€ вами&raquo;&nbsp;&mdash; ќтто фон Ѕисмарк.";

            var typografText = Typograf.Run(inputText).Result;

            Assert.AreEqual(outputText, typografText);
        }
    }
}