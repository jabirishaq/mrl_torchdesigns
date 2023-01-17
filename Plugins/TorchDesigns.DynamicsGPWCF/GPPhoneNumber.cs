using System.Text;

namespace Nop.Plugin.TorchDesigns.DynamicsGPWCF
{
    public static class GPPhoneNumber
    {
        public static string Convert(string str)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] >= '0' && str[i] <= '9')
                    sb.Append(str[i]);
            }

            return sb.ToString();
        }
    }
}
