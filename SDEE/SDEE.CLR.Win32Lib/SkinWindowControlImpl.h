#pragma once
#include <windows.h>

namespace SDEE {
	namespace CLI {
		namespace Win32Lib {
			public ref class SkinWindowControlImpl
			{
			public:
				property System::String^ Title;
				property System::Windows::Controls::Control^ AssociatedControl;
				property System::Windows::Thickness^ Borders;



			internal:
			};
		}
	}
}

