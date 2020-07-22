using NUnit.Framework;

namespace Portfolio.Typograf.Test
{
    public class Tests
    {
        [Test]
        public void Common1()
        {
            var inputText = "��� ����� ������� ��������� � ���������, ������� �������� ������� ���� ��������� � �������� �� �����. ������������ ��� ���������� �������� �������, � ����� ��� ��������� ����� ��������� �� ������ ����� �������.";
            var outputText = "��� ����� ������� ��������� �&nbsp;���������, ������� �������� ������� ���� ��������� �&nbsp;�������� ��&nbsp;�����. ������������ ��� ���������� �������� �������, �&nbsp;����� ��� ��������� ����� ��������� ��&nbsp;������ ����� �������.";

            var typografText = Typograf.Run(inputText).Result;

            Assert.AreEqual(outputText, typografText);
        }

        [Test]
        public void Common2()
        {
            var inputText = "� ������ ������ �������, ������ ����� �������, ������ ����� ����. ������ ������ ����� �������� �� ������ �����.";
            var outputText = "�&nbsp;������ ������ �������, ������ ����� �������, ������ ����� ����. ������ ������ ����� �������� ��&nbsp;������ �����.";

            var typografText = Typograf.Run(inputText).Result;

            Assert.AreEqual(outputText, typografText);
        }

        [Test]
        public void Common3()
        {
            var inputText = "� ������ ������ ����� ������, ���� ��� �� ������������ ������ 1. � ����������� ������� ����� � ����� ������ � �������������, �� �������� ���� ����� �� ������ ����� �������. � ������ ������������� � ����������� �������� ��-�� ������������� ������� �� ������-�� �������. � �� ������� ������, �� �� �� ������� ��������.";
            var outputText = "�&nbsp;������ ������ ����� ������, ���� ��� ��&nbsp;������������ ������ 1. �&nbsp;����������� ������� ����� �&nbsp;����� ������ �&nbsp;�������������, ��&nbsp;�������� ���� ����� ��&nbsp;������ ����� �������. �&nbsp;������ ������������� �&nbsp;����������� �������� ��-�� ������������� ������� ��&nbsp;������-�� �������. �&nbsp;��&nbsp;������� ������, ��&nbsp;��&nbsp;��&nbsp;������� ��������.";

            var typografText = Typograf.Run(inputText).Result;

            Assert.AreEqual(outputText, typografText);
        }

        [Test]
        public void CheckDash()
        {
            var inputText = "����� - �� �����!";
            var outputText = "�����&nbsp;&mdash; ��&nbsp;�����!";

            var typografText = Typograf.Run(inputText).Result;

            Assert.AreEqual(outputText, typografText);
        }

        [Test]
        public void CheckQuotes()
        {
            var inputText = "\"���� �� �� ����������� ���������, �� �������� ������� ����\" - ���� ��� �������.";
            var outputText = "&laquo;���� ��&nbsp;��&nbsp;����������� ���������, ��&nbsp;�������� ������� ����&raquo;&nbsp;&mdash; ���� ��� �������.";

            var typografText = Typograf.Run(inputText).Result;

            Assert.AreEqual(outputText, typografText);
        }

        [Test]
        public void CheckEncoding()
        {
            var inputText = "��� ����� ������� ��������� �&nbsp;���������, ������� �������� ������� ���� ��������� �&nbsp;�������� ��&nbsp;�����. ������������ ��� ���������� �������� �������, �&nbsp;����� ��� ��������� ����� ��������� ��&nbsp;������ ����� �������.";
            var outputText = "��� ����� ������� ��������� �&nbsp;���������, ������� �������� ������� ���� ��������� �&nbsp;�������� ��&nbsp;�����. ������������ ��� ���������� �������� �������, �&nbsp;����� ��� ��������� ����� ��������� ��&nbsp;������ ����� �������.";

            var typografText = Typograf.Run(inputText).Result;

            Assert.AreEqual(outputText, typografText);
        }
    }
}