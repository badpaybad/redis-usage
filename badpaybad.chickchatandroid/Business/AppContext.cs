namespace badpaybad.chickchatandroid.Business
{
   public static class AppContext
   {
       public static ChickChatServices ChickChatServices;

       public static string _ownerKey;
       public static string _selectedKey;
       public static ChickChatGroup _groupOwner;
       public static CustomTaskScheduler TaskScheduler;

       static AppContext()
       {
           ChickChatServices=new ChickChatServices();
            TaskScheduler=new CustomTaskScheduler();
       }

   }
}