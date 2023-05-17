namespace Dog_Identifier_Mobile.Models
{
    public interface IDogViewModel
    {
        string PhotoContentType { get; set; }
        byte[] PhotoData { get; set; }
    }
}