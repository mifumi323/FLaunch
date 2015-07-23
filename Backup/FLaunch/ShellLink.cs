using System;
using System.ComponentModel;
using System.Drawing;					// �R���\�[���v���W�F�N�g�̏ꍇ�́A�Q�Ƃ���ǉ����܂�
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;				// �R���\�[���v���W�F�N�g�̏ꍇ�́A�Q�Ƃ���ǉ����܂�

namespace util
{
/*
 <a href="http://www.vbaccelerator.com/home/The_Site/Usage_Policy/article.asp">Copyright</a>
 &#169; 2003 Steve McMahon <a href="mailto:steve@vbaccelerator.com">steve@vbaccelerator.com</a>.  All rights reserved.

  Copyright (http://www.vbaccelerator.com/home/The_Site/Usage_Policy/article.asp)
  2003 Steve McMahon steve@vbaccelerator.com.  All rights reserved.

  �A�C�R���@�\���폜�A#region ��� namespace
  �R�����g�i�e�L�g�E�ɖ󂵂Ă܂��j��ύX���ē]�ځB
  ���{��̃R�����g�͂��܂�M�p���Ȃ��ł������� (^^;
  �I���W�i���\�[�X�Ɋւ��Ă̓T�C�g�ivbaccelerator�j���Q�ƁB
*/
	/// <summary>
	/// IShellLink�C���^�[�t�F�C�X����A�I�u�W�F�N�g�𐶐�����B
	/// IShellLink �C���^�[�t�F�[�X�́A�V�F�������N���쐬�A�ύX���邽�߂̋@�\��񋟂��܂��B
	/// </summary>
	public class ShellLink : IDisposable
	{
#region InterfaceShellLink
		[ComImportAttribute()]
		[GuidAttribute("0000010C-0000-0000-C000-000000000046")]
		[InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
		private interface IPersist
		{
			[PreserveSig]
				//[helpstring("Returns the class identifier for the component object")]
			void GetClassID(out Guid pClassID);
		}

		[ComImportAttribute()]
		[GuidAttribute("0000010B-0000-0000-C000-000000000046")]
		[InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
		private interface IPersistFile
		{
			// can't get this to go if I extend IPersist, so put it here:
			[PreserveSig]
			void GetClassID(out Guid pClassID);

			//[helpstring("Checks for changes since last file write")]		
			void IsDirty();

			//[helpstring("Opens the specified file and initializes the object from its contents")]		
			void Load(
				[MarshalAs(UnmanagedType.LPWStr)] string pszFileName, 
				uint dwMode);

			//[helpstring("Saves the object into the specified file")]		
			void Save(
				[MarshalAs(UnmanagedType.LPWStr)] string pszFileName, 
				[MarshalAs(UnmanagedType.Bool)] bool fRemember);

			//[helpstring("Notifies the object that save is completed")]		
			void SaveCompleted(
				[MarshalAs(UnmanagedType.LPWStr)] string pszFileName);

			//[helpstring("Gets the current name of the file associated with the object")]		
			void GetCurFile(
				[MarshalAs(UnmanagedType.LPWStr)] out string ppszFileName);
		}

		[ComImportAttribute()]
		[GuidAttribute("000214EE-0000-0000-C000-000000000046")]
		[InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
		private interface IShellLinkA
		{
			//[helpstring("Retrieves the path and filename of a shell link object")]
			void GetPath(
				[Out(), MarshalAs(UnmanagedType.LPStr)] StringBuilder pszFile, 
				int cchMaxPath, 
				ref _WIN32_FIND_DATAA pfd, 
				uint fFlags);

			//[helpstring("Retrieves the list of shell link item identifiers")]
			void GetIDList(out IntPtr ppidl);

			//[helpstring("Sets the list of shell link item identifiers")]
			void SetIDList(IntPtr pidl);

			//[helpstring("Retrieves the shell link description string")]
			void GetDescription(
				[Out(), MarshalAs(UnmanagedType.LPStr)] StringBuilder pszFile,
				int cchMaxName);
		
			//[helpstring("Sets the shell link description string")]
			void SetDescription(
				[MarshalAs(UnmanagedType.LPStr)] string pszName);

			//[helpstring("Retrieves the name of the shell link working directory")]
			void GetWorkingDirectory(
				[Out(), MarshalAs(UnmanagedType.LPStr)] StringBuilder pszDir,
				int cchMaxPath);

			//[helpstring("Sets the name of the shell link working directory")]
			void SetWorkingDirectory(
				[MarshalAs(UnmanagedType.LPStr)] string pszDir);

			//[helpstring("Retrieves the shell link command-line arguments")]
			void GetArguments(
				[Out(), MarshalAs(UnmanagedType.LPStr)] StringBuilder pszArgs, 
				int cchMaxPath);

			//[helpstring("Sets the shell link command-line arguments")]
			void SetArguments(
				[MarshalAs(UnmanagedType.LPStr)] string pszArgs);

			//[propget, helpstring("Retrieves or sets the shell link hot key")]
			void GetHotkey(out short pwHotkey);
			//[propput, helpstring("Retrieves or sets the shell link hot key")]
			void SetHotkey(short pwHotkey);

			//[propget, helpstring("Retrieves or sets the shell link show command")]
			void GetShowCmd(out uint piShowCmd);
			//[propput, helpstring("Retrieves or sets the shell link show command")]
			void SetShowCmd(uint piShowCmd);

			//[helpstring("Retrieves the location (path and index) of the shell link icon")]
			void GetIconLocation(
				[Out(), MarshalAs(UnmanagedType.LPStr)] StringBuilder pszIconPath, 
				int cchIconPath, 
				out int piIcon);
		
			//[helpstring("Sets the location (path and index) of the shell link icon")]
			void SetIconLocation(
				[MarshalAs(UnmanagedType.LPStr)] string pszIconPath, 
				int iIcon);

			//[helpstring("Sets the shell link relative path")]
			void SetRelativePath(
				[MarshalAs(UnmanagedType.LPStr)] string pszPathRel, 
				uint dwReserved);

			//[helpstring("Resolves a shell link. The system searches for the shell link object and updates the shell link path and its list of identifiers (if necessary)")]
			void Resolve(
				IntPtr hWnd, 
				uint fFlags);

			//[helpstring("Sets the shell link path and filename")]
			void SetPath(
				[MarshalAs(UnmanagedType.LPStr)] string pszFile);
		}


		[ComImportAttribute()]
		[GuidAttribute("000214F9-0000-0000-C000-000000000046")]
		[InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
		private interface IShellLinkW
		{
			//[helpstring("Retrieves the path and filename of a shell link object")]
			void GetPath(
				[Out(), MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszFile, 
				int cchMaxPath, 
				ref _WIN32_FIND_DATAW pfd, 
				uint fFlags);

			//[helpstring("Retrieves the list of shell link item identifiers")]
			void GetIDList(out IntPtr ppidl);

			//[helpstring("Sets the list of shell link item identifiers")]
			void SetIDList(IntPtr pidl);

			//[helpstring("Retrieves the shell link description string")]
			void GetDescription(
				[Out(), MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszFile,
				int cchMaxName);
		
			//[helpstring("Sets the shell link description string")]
			void SetDescription(
				[MarshalAs(UnmanagedType.LPWStr)] string pszName);

			//[helpstring("Retrieves the name of the shell link working directory")]
			void GetWorkingDirectory(
				[Out(), MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszDir,
				int cchMaxPath);

			//[helpstring("Sets the name of the shell link working directory")]
			void SetWorkingDirectory(
				[MarshalAs(UnmanagedType.LPWStr)] string pszDir);

			//[helpstring("Retrieves the shell link command-line arguments")]
			void GetArguments(
				[Out(), MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszArgs, 
				int cchMaxPath);

			//[helpstring("Sets the shell link command-line arguments")]
			void SetArguments(
				[MarshalAs(UnmanagedType.LPWStr)] string pszArgs);

			//[propget, helpstring("Retrieves or sets the shell link hot key")]
			void GetHotkey(out short pwHotkey);
			//[propput, helpstring("Retrieves or sets the shell link hot key")]
			void SetHotkey(short pwHotkey);

			//[propget, helpstring("Retrieves or sets the shell link show command")]
			void GetShowCmd(out uint piShowCmd);
			//[propput, helpstring("Retrieves or sets the shell link show command")]
			void SetShowCmd(uint piShowCmd);

			//[helpstring("Retrieves the location (path and index) of the shell link icon")]
			void GetIconLocation(
				[Out(), MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszIconPath, 
				int cchIconPath, 
				out int piIcon);
		
			//[helpstring("Sets the location (path and index) of the shell link icon")]
			void SetIconLocation(
				[MarshalAs(UnmanagedType.LPWStr)] string pszIconPath, 
				int iIcon);

			//[helpstring("Sets the shell link relative path")]
			void SetRelativePath(
				[MarshalAs(UnmanagedType.LPWStr)] string pszPathRel, 
				uint dwReserved);

			//[helpstring("Resolves a shell link. The system searches for the shell link object and updates the shell link path and its list of identifiers (if necessary)")]
			void Resolve(
				IntPtr hWnd, 
				uint fFlags);

			//[helpstring("Sets the shell link path and filename")]
			void SetPath(
				[MarshalAs(UnmanagedType.LPWStr)] string pszFile);
		}

		[GuidAttribute("00021401-0000-0000-C000-000000000046")]
		[ClassInterfaceAttribute(ClassInterfaceType.None)]
		[ComImportAttribute()]
		private class CShellLink{}
#endregion
#region enumAndStructShellLink
		private enum EShellLinkGP : uint
		{
			SLGP_SHORTPATH = 1,
			SLGP_UNCPRIORITY = 2
		}

		[Flags]
		private enum EShowWindowFlags : uint
		{
			SW_HIDE = 0,
			SW_SHOWNORMAL = 1,
			SW_NORMAL = 1,
			SW_SHOWMINIMIZED = 2,
			SW_SHOWMAXIMIZED = 3,
			SW_MAXIMIZE = 3,
			SW_SHOWNOACTIVATE = 4,
			SW_SHOW = 5,
			SW_MINIMIZE = 6,
			SW_SHOWMINNOACTIVE = 7,
			SW_SHOWNA = 8,
			SW_RESTORE = 9,
			SW_SHOWDEFAULT = 10,
			SW_MAX = 10
		}


		[StructLayoutAttribute(LayoutKind.Sequential, Pack=4, Size=0, CharSet=CharSet.Unicode)]
		private struct _WIN32_FIND_DATAW
		{
			public uint dwFileAttributes;
			public _FILETIME ftCreationTime;
			public _FILETIME ftLastAccessTime;
			public _FILETIME ftLastWriteTime;
			public uint nFileSizeHigh;
			public uint nFileSizeLow;
			public uint dwReserved0;
			public uint dwReserved1;
			[MarshalAs(UnmanagedType.ByValTStr , SizeConst = 260)] // MAX_PATH
			public string cFileName;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 14)]
			public string cAlternateFileName;
		}

		[StructLayoutAttribute(LayoutKind.Sequential, Pack=4, Size=0, CharSet=CharSet.Ansi)]
		private struct _WIN32_FIND_DATAA
		{
			public uint dwFileAttributes;
			public _FILETIME ftCreationTime;
			public _FILETIME ftLastAccessTime;
			public _FILETIME ftLastWriteTime;
			public uint nFileSizeHigh;
			public uint nFileSizeLow;
			public uint dwReserved0;
			public uint dwReserved1;
			[MarshalAs(UnmanagedType.ByValTStr , SizeConst = 260)] // MAX_PATH
			public string cFileName;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 14)]
			public string cAlternateFileName;
		}

		[StructLayoutAttribute(LayoutKind.Sequential, Pack=4, Size=0)]
		private struct _FILETIME 
		{
			public uint dwLowDateTime;
			public uint dwHighDateTime;
		}

		private class UnManagedMethods
		{
			[DllImport("Shell32", CharSet=CharSet.Auto)]
			internal extern static int ExtractIconEx (
				[MarshalAs(UnmanagedType.LPTStr)] 
				string lpszFile,
				int nIconIndex,
				IntPtr[] phIconLarge, 
				IntPtr[] phIconSmall,
				int nIcons);

			[DllImport("user32")]
			internal static extern int DestroyIcon(IntPtr hIcon);
		}

		/// <summary>
		/// �^�[�Q�b�g������ꂽ�ꍇ�A�����N���ǂ̂悤�ɉ�������邩���߂�t���O�B
		/// </summary>
		[Flags]
		public enum EShellLinkResolveFlags : uint
		{
			/// <summary>
			/// Allow any match during resolution.  Has no effect
			/// on ME/2000 or above, use the other flags instead.
			/// �C�ӂ̑g�ݍ��킹��������B
			/// Win ME/2000 �������͂���ȍ~��OS�ł͌��ʂ��Ȃ�
			/// ���̃t���O�����g���Ă��������B
			/// </summary>
			SLR_ANY_MATCH = 0x2,

			/// <summary>
			/// Call the Microsoft Windows Installer. 
			/// </summary>
			SLR_INVOKE_MSI = 0x80,

			/// <summary>
			/// distributed(���U) link tracking(�ǐ�)��s�\�ɂ��Ă��������B
			/// �f�t�H���g�ł́Adistributed link tracking�́A
			/// �{�����[�����Ɋ�Â��������̑��u�⃊���[�o�u���E���f�B�A��
			/// �ǐՂ��܂��B���̃h���C�u���^�[���ς�������u�̃t�@�C���E�V�X�e����
			/// �ǐՂ��邽�߂�UNC�̃p�X���g�p���܂��B
			/// SLR_NOLINKINFO�̃Z�b�g�͗����̃^�C�v�̃g���b�L���O��s�\�ɂ��܂��B
			/// </summary>
			SLR_NOLINKINFO = 0x40,

			/// <summary>
			/// �����N���������邱�Ƃ��ł��Ȃ��ꍇ�ɁA�_�C�A���O�E�{�b�N�X��\�����Ȃ��B
			/// SLR_NO_UI���Z�b�g����ꍇ�A�����N�����ɔ�₳���ő�̎��Ԃ��w�肵�܂��B
			/// �^�C���A�E�g�l�̓~���Z�J���h�Ŏw�肷�邱�Ƃ��ł��܂��B
			/// �^�C���A�E�g�������Ƀ����N���������邱�Ƃ��ł��Ȃ��ꍇ�A���䂪�Ԃ�܂��B
			/// �^�C���A�E�g���Z�b�g����Ȃ��ꍇ�A�f�t�H���g�l�� 3,000 �~���Z�J���h(3�b)���Z�b�g����܂��B
			/// </summary>										    
			SLR_NO_UI = 0x1,

			/// <summary>
			/// SDK�̒��Ńh�L�������g������ĂȂ�(�Ȃ񂶂Ⴛ���)�B
			/// SLR_NO_UI�Ɠ����ł���Ɖ��肷�邪�AhWnd�̂Ȃ��K�p�̂��߂Ɛ����B
			/// </summary>
			SLR_NO_UI_WITH_MSG_PUMP = 0x101,

			/// <summary>
			/// �����N�����X�V���Ȃ��B
			/// </summary>
			SLR_NOUPDATE = 0x8,

			/// <summary>
			/// �T���q���[���X�e�B�b�N�X�����s���Ȃ��B
			/// </summary>
			SLR_NOSEARCH = 0x10,

			/// <summary>
			/// distributed(���U?) link tracking�g�p���Ȃ��B
			/// </summary>
			SLR_NOTRACK = 0x20,

			/// <summary>
			/// �����N�E�I�u�W�F�N�g���ς�����ꍇ�́A�Ώۂ̃p�X����у��X�g���X�V���Ă��������B
			/// SLR_UPDATE ���Z�b�g�����ꍇ�A�����N�E�I�u�W�F�N�g���ς�������ǂ�����
			/// ���ׂ邽�߂� IPersistFile::IsDirty ���Ăяo���K�v������܂���B
			/// </summary>
			SLR_UPDATE  = 0x4
		}

		public enum LinkDisplayMode : uint
		{
			edmNormal = EShowWindowFlags.SW_NORMAL,
			edmMinimized = EShowWindowFlags.SW_SHOWMINNOACTIVE,
			edmMaximized = EShowWindowFlags.SW_MAXIMIZE
		}

#endregion
#region maincode
		// Use Unicode (W) under NT, otherwise use ANSI
		private IShellLinkW linkW;
		private IShellLinkA linkA;
		private string shortcutFile = "";

		/// <summary>
		/// �V�F���E�����N�E�I�u�W�F�N�g�̃C���X�^���X���쐬���܂��B
		/// </summary>
		public ShellLink()
		{
			if (System.Environment.OSVersion.Platform == PlatformID.Win32NT)
			{
				linkW = (IShellLinkW)new CShellLink();
			}
			else
			{
				linkA = (IShellLinkA)new CShellLink();
			}
		}

		/// <summary>
		/// �w�肳�ꂽ�����N�t�@�C������
		/// �V�F���E�����N�E�I�u�W�F�N�g�̃C���X�^���X���쐬���܂��B
		/// </summary>
		/// <param name="linkFile">�I�[�v�����郊���N�t�@�C��</param>
		public ShellLink(string linkFile) : this()
		{
			Open(linkFile);
		}

		/// <summary>
		/// Dispose()���Ăяo����Ă��Ȃ��ꍇ�A������Ăяo���܂��B
		/// </summary>
		~ShellLink()
		{
			Dispose();
		}

		/// <summary>
		/// COM ShellLink�I�u�W�F�N�g�������[�X���āA�I�u�W�F�N�g���������܂�
		/// </summary>
		public void Dispose()
		{
			if (linkW != null ) 
			{
				Marshal.ReleaseComObject(linkW);
				linkW = null;
			}
			if (linkA != null)
			{
				Marshal.ReleaseComObject(linkA);
				linkA = null;
			}
		}

		public string ShortCutFile
		{
			get
			{
				return this.shortcutFile;
			}
			set
			{
				this.shortcutFile = value;
			}
		}

		/// <summary>
		/// �����N���ݒ�A�擾
		/// </summary>
		public string Target
		{
			get
			{		
				StringBuilder target = new StringBuilder(260, 260);
				if (linkA == null)
				{
					_WIN32_FIND_DATAW fd = new _WIN32_FIND_DATAW();
					linkW.GetPath(target, target.Capacity, ref fd, (uint)EShellLinkGP.SLGP_UNCPRIORITY);
				}
				else
				{
					_WIN32_FIND_DATAA fd = new _WIN32_FIND_DATAA();
					linkA.GetPath(target, target.Capacity, ref fd, (uint)EShellLinkGP.SLGP_UNCPRIORITY);
				}
				return target.ToString();
			}
			set
			{
				if (linkA == null)
				{
					linkW.SetPath(value);
				}
				else
				{
					linkA.SetPath(value);
				}
			}
		}

		/// <summary>
		/// ��ƃf�B���N�g����ݒ�A�擾
		/// </summary>
		public string WorkingDirectory
		{
			get
			{
				StringBuilder path = new StringBuilder(260, 260);
				if (linkA == null)
				{
					linkW.GetWorkingDirectory(path, path.Capacity);
				}
				else
				{
					linkA.GetWorkingDirectory(path, path.Capacity);
				}
				return path.ToString();
			}
			set
			{
				if (linkA == null)
				{
					linkW.SetWorkingDirectory(value);	
				}
				else
				{
					linkA.SetWorkingDirectory(value);
				}
			}
		}

		/// <summary>
		/// �����N�̋L�q�i�v���p�e�B�[�Ō���ƁA�R�����g�j��ݒ�A�擾
		/// </summary>
		public string Description
		{
			get
			{
				StringBuilder description = new StringBuilder(1024, 1024);
				if (linkA == null)
				{
					linkW.GetDescription(description, description.Capacity);
				}
				else
				{
					linkA.GetDescription(description, description.Capacity);
				}
				return description.ToString();
			}
			set
			{
				if (linkA == null)
				{
					linkW.SetDescription(value);
				}
				else
				{
					linkA.SetDescription(value);
				}
			}
		}

		/// <summary>
		/// �R�}���h���C����������ݒ�A�擾
		/// </summary>
		public string Arguments
		{
			get
			{				
				StringBuilder arguments = new StringBuilder(260, 260);
				if (linkA == null)
				{
					linkW.GetArguments(arguments, arguments.Capacity);
				}
				else
				{
					linkA.GetArguments(arguments, arguments.Capacity);
				}
				return arguments.ToString();
			}
			set
			{
				if (linkA == null)
				{
					linkW.SetArguments(value);
				}
				else
				{
					linkA.SetArguments(value);
				}
			}
		}

		/// <summary>
		/// �ߓ������s�����ꍇ�A�����̃f�B�X�v���C�E���[�h��ݒ�A�擾
		/// </summary>
		public LinkDisplayMode DisplayMode
		{
			get
			{
				uint cmd = 0;
				if (linkA == null)
				{
					linkW.GetShowCmd(out cmd);
				}
				else
				{
					linkA.GetShowCmd(out cmd);
				}
				return (LinkDisplayMode)cmd;
			}
			set
			{
				if (linkA == null)
				{
					linkW.SetShowCmd((uint)value);
				}
				else
				{
					linkA.SetShowCmd((uint)value);
				}
			}
		}

		/// <summary>
		/// �V���[�g�J�b�g�̃z�b�g�L�[��ݒ�A�擾
		/// [���z�L�[�R�[�h=>System.Windows.Forms.Keys]
		/// </summary>
		public Keys HotKey
		{
			get
			{
				short key = 0;
				if (linkA == null)
				{
					linkW.GetHotkey(out key);
				}
				else
				{
					linkA.GetHotkey(out key);
				}
				return (Keys)key;
			}
			set
			{
				if (linkA == null)
				{
					linkW.SetHotkey((short)value);
				}
				else
				{
					linkA.SetHotkey((short)value);
				}
			}
		}

		/// <summary>
		/// ShortCutFile(�v���p�e�B�[) �ɃV���[�g�J�b�g�t�@�C����ۑ�����
		/// </summary>
		public void Save()
		{
			Save(shortcutFile);
		}

		/// <summary>
		/// �V���[�g�J�b�g�t�@�C����ۑ�����
		/// </summary>
		/// <param name="linkFile">�V���[�g�J�b�g�̕ۑ���(.lnk)</param>
		public void Save(string linkFile)
		{   
			// Save the object to disk
			if (linkA == null)
			{
				((IPersistFile)linkW).Save(linkFile, true);
				shortcutFile = linkFile;
			}
			else
			{
				((IPersistFile)linkA).Save(linkFile, true);
				shortcutFile = linkFile;
			}
		}

		/// <summary>
		/// �w�肳�ꂽ�V���[�g�J�b�g�t�@�C�������[�h����
		/// </summary>
		/// <param name="linkFile">�ǂݍ��݃V���[�g�J�b�g�t�@�C��(.lnk)</param>
		public void Open(string linkFile)
		{
			Open(linkFile, IntPtr.Zero, (EShellLinkResolveFlags.SLR_ANY_MATCH | EShellLinkResolveFlags.SLR_NO_UI), 1);
		}

		/// <summary>
		/// �w�肳�ꂽ�V���[�g�J�b�g�t�@�C�������[�h���A
		/// �V���[�g�J�b�g�̃^�[�Q�b�g���Z�b�g�Ɍ�����Ȃ��ꍇ�A
		/// UI ������R���g���[������t���O��������B
		/// </summary>
		/// <param name="linkFile">�ǂݍ��݃V���[�g�J�b�g�t�@�C��(.lnk)</param>
		/// <param name="hWnd">(���������)�K�p��UI�̃E�B���h�E�n���h��</param>
		/// <param name="resolveFlags">������w�肷��t���O</param>
		public void Open(string linkFile, IntPtr hWnd, EShellLinkResolveFlags resolveFlags)
		{
			Open(linkFile, hWnd, resolveFlags, 1);
		}

		/// <summary>
		/// �w�肳�ꂽ�V���[�g�J�b�g�t�@�C�������[�h���A
		/// �V���[�g�J�b�g�̃^�[�Q�b�g���Z�b�g�Ɍ�����Ȃ��ꍇ�A
		/// UI ������R���g���[������t���O��������B
		/// SLR_NO_UI���w�肳��Ȃ��ꍇ�A
		/// ����ɁA�^�C���A�E�g���w�肷�邱�Ƃ��ł���B
		/// </summary>
		/// <param name="linkFile">�ǂݍ��݃V���[�g�J�b�g�t�@�C��(.lnk)</param>
		/// <param name="hWnd">(���������)�K�p��UI�̃E�B���h�E�n���h��</param>
		/// <param name="resolveFlags">������w�肷��t���O</param>
		/// <param name="timeOut">SLR_NO_UI���~���Z�J���h�Ŏw�肳���ꍇ�A�^�C���A�E�g</param>
		public void Open(string linkFile, IntPtr hWnd, EShellLinkResolveFlags resolveFlags, ushort timeOut)
		{
			uint flags;

			if ((resolveFlags & EShellLinkResolveFlags.SLR_NO_UI) == EShellLinkResolveFlags.SLR_NO_UI)
			{
				flags = (uint)((int)resolveFlags | (timeOut << 16));
			}
			else
			{
				flags = (uint)resolveFlags;
			}

			if (linkA == null)
			{
				((IPersistFile)linkW).Load(linkFile, 0); //STGM_DIRECT)
				linkW.Resolve(hWnd, flags);
				this.shortcutFile = linkFile;
			}
			else
			{
				((IPersistFile)linkA).Load(linkFile, 0); //STGM_DIRECT)
				linkA.Resolve(hWnd, flags);
				this.shortcutFile = linkFile;
			}
		}
#endregion
	}

}
