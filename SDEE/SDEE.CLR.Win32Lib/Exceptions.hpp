#pragma once

namespace SDEE {
	namespace CLI {
		namespace Win32Lib
		{
			public ref class SkinEngineException : public System::Exception
			{
			public:
				__inline SkinEngineException() : System::Exception() {}
				inline SkinEngineException(System::String^ message) : System::Exception(message) {}
			};

			public ref class WindowCreationException : public System::Exception
			{
			public:
				__inline WindowCreationException() : System::Exception() {}
				inline WindowCreationException(System::String^ message) : System::Exception(message) {}
				__inline WindowCreationException(System:: UInt64 errorCode, System::String^ message) : System::Exception(message + L" ; ERROR CODE " + errorCode.ToString()) {}
				inline WindowCreationException(System::UInt64 errorCode) : WindowCreationException(errorCode, System::String::Empty) {}
			};
		}
	}
}