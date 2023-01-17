namespace Nop.Plugin.TorchDesigns.DynamicsGPWCF
{
    public static class GPName
    {
        /// <summary>
        /// Cleans a name for GP by removing / replacing special character sequences.
        /// </summary>
        /// <param name="StrName"></param>
        /// <returns>Cleaned Name</returns>
        public static string CleanName(string StrName)
        {
            string strName = StrName.ToUpper().Trim();

            strName = strName.Replace("&", "AND");

            return strName;
        }
    }
}
