   M S B A M L   `   `   `  ���� H  DPlate.Trainer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:Plate.TrainerPlate.Trainer      Plate.Trainer.MainWindow   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     -localclr-namespace:Plate.Trainer     -     � Title$  
MainWindow��$	��800��$
��1400�� � 	Resources  �� 
TargetType   
baseButton �� Property �� Value �� BasedOn  CrossButton  FullTextBox��%A  &	        '�� #     7   &	 2     '� �     '�� �     &	 �     �� " ��.���  � ��$ 10q��� " ��8   �� " ��.���  � ��$ 20����  � ��$ 20����  � ��$ 7,0,0,0q��� " ��� " ��.���  � ��$ 1q���  � ��$ 90���� " ��.���  � ��$ 1q���  � ��$	 152�� J� CommandBindings )  #System.Windows.Input.CommandBinding  -      Plate.Trainer.MainWindow   PrevCommand   Command# Z   -   	   NextCommand# Z	 .��� 
 � ColumnDefinitions
 �� $	��200 ���  � RowDefinitions �� $��70 ��� .+���  �� Orientation$ 
Horizontal=�$��0��$��2��.+��� -   $��50��.���� $��30��$��30��
���iii� J� OpacityMask !�  j� Stretch$
 Fill=� 
 folder !� Visual# [  � ToolTip$ Выбрать папку���� -    ��Name$ ZoomText���� -    ��Name$ ThumbnailList��$��1��$��0������ *Images+���� .R��  �� $	��100 ��� $��20 �.+��� $��0��$	��150������ *Annotation.FullName+�� $��1������ *Annotation.FileName+� -    �Name$ GridMain��$��1��$��1��
 �� �� $	��200 � �� $��40 ��� $
��412* ��� $��80 �.+��� -    ��Name$ ScrollViewer��$��Visible=�$��Visible=�$��0��$��2��$��0��.��� -	   $ 	GridImage��.+��� -
    ��Name$ SourceImage��$��0��$��0��}�. $	��Top=�$
��Left=��� -    ��Name$ Canvas��$��0��$��0��}�. 
������ $
��Left=�$	��Top=��� -    ��Name$ 	AddRegion��$��1��$��0��.��!Добавить область�� -   $ RegionsList��$��1��$��1������ *CurrentItemRegions+���� .R��  �� �� �� �� �� 
 �� �� .+�� $��0��$��0��$��2��
 �� �� $��30 �.+��� #��[ ���� *Name+ �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=��� -   $��1��#��[ .��✕�� -   $��1��$��0������ *StartX+$ TwoWay=�$ PropertyChanged=��� -   $��1��$��1������ *StartY+$ TwoWay=�$ PropertyChanged=��� -   $��2��$��0������ *EndX+$ TwoWay=�$ PropertyChanged=��� -   $��2��$��1������ *EndY+$ TwoWay=�$ PropertyChanged=��� $��	Truncated��$��3��$��0���� $��3��$��1�� d� 	IsChecked �� *	Truncated+$ PropertyChanged=��� $��	Difficult��$��4��$��0���� $��4��$��1�� �� *	Difficult+$ PropertyChanged=�� $��3��$��0��$��2��
 �� �� .+��� -   $ ButtonPrevious��.��Предыдущая�� -   $ 
ButtonNext��$��1��.��Следующая