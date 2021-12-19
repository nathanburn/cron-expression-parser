namespace CronExpressionParser.Models
{
    public class BaseArgument
    {
        private static int LABEL_COLUMN_LENGTH = 14;

        public string Label;

        public string Input = string.Empty;

        public BaseArgument(string label)
        {
            this.Label = label;
        }

        public virtual void SetInput(string value)
        {
            this.Input = value;
        }

        public override string ToString()
        {
            return $"{GetLabelWithWhiteSpaces()}{Input}";
        }

        protected string GetLabelWithWhiteSpaces()
        {
            return $"{Label}{new string(' ', LABEL_COLUMN_LENGTH - Label.Length)}";
        }
    }
}