   M S B A M L   `   `   `  ����M  ITestFunctionGW040x, Version=1.0.0.6, Culture=neutral, PublicKeyToken=null7 clr-namespace:TestFunctionGW040xTestFunctionGW040x  5   	   !    TestFunctionGW040x.frmPOWER   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   2local clr-namespace:TestFunctionGW040x     5   	   -   5          �Name$  wdPower��5   	    � Title$ frmPOWER��5	   	   $	��120��6   $	��450��6'      customWindow#��[  63    � WindowStartupLocation$ CenterScreen=�6Y    � ShowInTaskbar .   � ShowActivated .   � Topmost . .��5
      �� ���#4035e8 �� CornerRadius$ 5��6"     colorBorder#��[ 63   $��1q�6^   ._�5   
   �  � RowDefinitions 5      �� 5      $��40 �6    �� 5      $��* �6    .+�5      � $��0��6   .+�5      � .+�5      �� -   5         ��Name$ 	lblLegend��5      $��15(�6/   $��Center=�6=   $��Center=�6_   
��� ���$��40��6�   $��SemiBold"�6�   ��6�   �� 	 �� Path$	 
PowerTitle�
 �� UpdateSourceTrigger$
 PropertyChanged=�6�   5      �� 5      -   5        $��Center=�5   5   $��20(�6P   $
��Bold"�6^   $��0,-5,5,0q�6p   $��Right=�6�     colorTextTitle#��[ 6�    ^� ToolTip$ 
Tắt Form��6�   $��X��6�   6�   5      �� 5      $ 5��6   $��1��6&   $��5,0,5,10q�63   #��[ 6E   $��1q�6p   ._�5      � $��	10,5,10,5q�6   .+�5      � $��Stretch=�6    $��Stretch=�6<   .+�5      �� $��/Assets/Images/power3.png��6    6E   �� 5      $��25(�6    $��Center=�6.   $��SemiBold"�6P   
�������.��6y   THỜI GIAN CÒN LẠI6�   �� 5      $ 30��6!   
��� ���$��1q�6H   $��55��6\   ._�5      � .+�5   "   �� $��Center=�6n   $��Center=�6�   $��25(�6�   $��Center=�6�   $
��Bold"�6�   
���  ����6(   �� $	 TimeOut�$
 PropertyChanged=�6�   5      5      �� 5!      $��25(�6    $��Center=�6.   $��SemiBold"�6P   
�������.��6y   GIÂY6   5"      5#      5$      5%      5&      5'      