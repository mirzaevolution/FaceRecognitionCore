   M S B A M L   `   `   `  ����S  OFaceRecognition.GUILayer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullm;clr-namespace:FaceRecognition.GUILayer.Authentication.Login-FaceRecognition.GUILayer.Authentication.Login  5      =    7FaceRecognition.GUILayer.Authentication.Login.LoginView   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      H DMahApps.Metro, Version=1.6.5.1, Culture=neutral, PublicKeyToken=null:control,http://metro.mahapps.com/winfx/xaml/controls  5      Glocal;clr-namespace:FaceRecognition.GUILayer.Authentication.Login  5      .��5	      � .+�5
   
   � $��Stretch=�6   $��Center=�6-   $��5,5,20,5q�6H     � DataContext  6Z   �� *User+ � ColumnDefinitions 5      �� 5      $
��auto �6#   �� 5       � RowDefinitions 5      �� 5      $
��auto �6    �� 5      $
��auto �6    �� 5      $
��auto �6    �� 5      $
��auto �6    .+�5      �� $!��/Resources/Icons/shield.png��6   $��34��69   $��44��6E   $��10q�6P   $	��Top=�6\   $��4��6t   �� 5      *  $MahApps.Metro.Controls.TextBoxHelper   	Watermark$ Enter your User Name��6T      ClearTextButton . $��1��6�   $��0,4,0,4q�6�      HasText .  ��6   �� *
UserName+ �� UpdateSourceTrigger$ PropertyChanged=�6�   G� 5      -   5        $��PasswordBoxField��5      $ Enter your Password��6\    . $��1��6�   $��1��6�   $��0,4,0,4q�6�   �� 5       �� Orientation$ 
Horizontal=�6   $��2��62   $��1��6?   $��5,5,0,0q�6O   $��Right=�6`   .+�5      �� $��Login��6   $��80��6)   )  #MahApps.Metro.Controls.ButtonHelper   CornerRadius$ 10��5      $��2q�6@   
���  ��$	��1.5q�6]   ��5      �� 	 �� RelativeSource	 �� 
 �� AncestorType$
 UserControlL� �� Path$ DataContext.LoginCommand�5   ]   �� 5      $
��Exit��6   $��80��6(   $ 10��5      $��2,2,2,0q�6@   
���  ��$	��1.5q�6c   ��5      �� 	 �� $
 UserControlL�$ DataContext.ExitCommand�5   c   5       �� 5!      $��3��6   $��2��6%   $��	20,20,5,5q�69   Don't have any account yet? �� 5"   .    �� Command 68   �� 	 �� $
 UserControlL�$% DataContext.GoToRegisterCommand�.�6�   	Register!6�   5#      5$      5%      5&      