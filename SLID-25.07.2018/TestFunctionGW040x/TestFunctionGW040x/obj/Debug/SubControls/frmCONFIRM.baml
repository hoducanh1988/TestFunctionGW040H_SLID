   M S B A M L   `   `   `  ����M  ITestFunctionGW040x, Version=1.0.0.6, Culture=neutral, PublicKeyToken=null7 clr-namespace:TestFunctionGW040xTestFunctionGW040x  5   	   #    TestFunctionGW040x.frmCONFIRM   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   2local clr-namespace:TestFunctionGW040x     5   	     � Title$  
frmCONFIRM��5   	   $	��200��6   $	��350��6)    � WindowStartupLocation$ CenterScreen=�65      customWindow#��[  6Z    � ShowInTaskbar .   � ShowActivated .   � Topmost . .��5	      �� -   5        ���	#FF444444$��1q�5	   $     colorBorder#��[ 68    �� CornerRadius$ 5��6c   ._�5
   
   �  � RowDefinitions 5      �� 5      $��40 �6    �� 5      $��* �6    .+�5      � $��0��6   .+�5      � .+�5      �� $��15(�6   $��Center=�6*   $��Center=�6L   
�������$��40��6z   $��SemiBold"�6�   .��6�   .,XÁC NHẬN THÔNG TIN NGƯỜI QUẢN TRỊ6�   5      �� 5      -   5        $��Center=�5   5   $��20(�6P   $
��Bold"�6^   $��0,-5,5,0q�6p   $��Right=�6�     colorTextTitle#��[ 6�    ^� ToolTip$ 
Tắt Form��6�   $��X��6�   6�   5      �� 5      $ 5��6   $��1��6&   $��5,0,5,5q�63   #��[ 6D   $��1q�6o   ._�5      � $��
10,10,10,0q�6   .+�5      �� .+�5      �� $��0,0,0,5q�6    
�������.��6D   ,*Nhập mật khẩu người quản trị:6c   G� 5      -   5         G�Name$ passwordbox��5   r     BackgroundReady#��[ 6�   $��30��6�   $��20(�6�   	 G� PasswordChar$	 *��6�   $��Center=�6�   $
��Left=�6  

 � Tag$
 Input administrator here��6$  6E  � 5      $��Right=�6$   $��0,5,0,0q�6@   .+�5      �� -   5        $��30��5   :   $��80��6F   ���	#FFAAAAAA$��Center=�6h   $��0,0,10,0q�6�   .��6�   
Hủy Bỏ6�   �� 5      -   5        $��30��5   :   $��80��6F   ���	#FFAAAAAA$��Center=�6h   .��6�   
Đồng Ý6�   5      �� 5      -   5         ��Name$ 	tbMessage��5   $   $��0,5,0,0q�67   $��13(�6H     colorTextNG#��[ 6V   $
��Wrap=�6�   6�   5      5       5!      5#      5$      5%      