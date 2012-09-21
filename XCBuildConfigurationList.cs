using UnityEngine;
using System.Collections;

namespace UnityEditor.XCodeEditor
{
	public class XCBuildConfigurationList : PBXType
	{
		protected const string BUILDSETTINGS_KEY = "buildSettings";
		protected const string HEADER_SEARCH_PATH_KEY = "HEADER_SEARCH_PATH";
		protected const string LIBRARY_SEARCH_PATH_KEY = "LIBRARY_SEARCH_PATH";
		protected const string OTHER_C_FLAGS_KEY = "OTHER_CFLAGS";
		
		public bool AddSearchPaths( string path, string basePath, string key, bool recursive = true )
		{
			PBXList paths = new PBXList();
			paths.Add( path );
			return AddSearchPaths( paths, basePath, key, recursive );
		}
		
		public bool AddSearchPaths( PBXList paths, string basePath, string key, bool recursive = true )
		{
			bool modified = false;
			
			if( !ContainsKey( basePath ) )
				this.Add( basePath, new PBXDictionary() );
			
			foreach( string path in paths ) {
				string currentPath = path;
				if( recursive && !path.EndsWith( "/**" ) )
					currentPath += "**";
				
				if( !((PBXDictionary)this[basePath]).ContainsKey( key ) ) {
					((PBXDictionary)this[basePath]).Add( key, new PBXList() );
				}
				else if( ((PBXDictionary)this[basePath])[key] is string ) {
					PBXList list = new PBXList();
					list.Add( ((PBXDictionary)this[basePath])[key] );
					((PBXDictionary)this[basePath])[key] = list;
				}
				
				if( ((PBXList)((PBXDictionary)this[basePath])[key]).Add( "\"" + currentPath + "\"" ) >= 0 ) {
					modified = true;
				}
			}
		
			return modified;
		}
		
		public bool AddHeaderSearchPaths( PBXList paths, bool recursive = true )
		{
			return this.AddSearchPaths( paths, "buildSettings", "HEADER_SEARCH_PATHS", recursive );
		}
		
		public bool AddLibrarySearchPath( PBXList paths, bool recursive = true )
		{
			return this.AddSearchPaths( paths, "buildSettings", "LIBRARY_SEARCH_PATHS", recursive );
		}
		
		public bool AddOtherCFlags( string flag )
		{
			PBXList flags = new PBXList();
			flags.Add( flag );
			return AddOtherCFlags( flags );
		}
		
		public bool AddOtherCFlags( PBXList flags )
		{
			bool modified = false;
			
			if( !ContainsKey( BUILDSETTINGS_KEY ) )
				this.Add( BUILDSETTINGS_KEY, new PBXDictionary() );
			
			foreach( string flag in flags ) {
				
				if( !((PBXDictionary)this[BUILDSETTINGS_KEY]).ContainsKey( OTHER_C_FLAGS_KEY ) ) {
					((PBXDictionary)this[BUILDSETTINGS_KEY]).Add( OTHER_C_FLAGS_KEY, new PBXList() );
				}
				else if ( ((PBXDictionary)this[BUILDSETTINGS_KEY])[ OTHER_C_FLAGS_KEY ] is string ) {
					string tempString = (string)((PBXDictionary)this[BUILDSETTINGS_KEY])[OTHER_C_FLAGS_KEY];
					((PBXDictionary)this[BUILDSETTINGS_KEY])[ OTHER_C_FLAGS_KEY ] = new PBXList();
					((PBXList)((PBXDictionary)this[BUILDSETTINGS_KEY])[OTHER_C_FLAGS_KEY]).Add( tempString );
				}
				
				if( ((PBXList)((PBXDictionary)this[BUILDSETTINGS_KEY])[OTHER_C_FLAGS_KEY]).Add( flag ) >= 0 ) {
					modified = true;
				}
			}
			
			return modified;
		}
		
//	class XCBuildConfiguration(PBXType):
//    def add_search_paths(self, paths, base, key, recursive=True):
//        modified = False
//
//        if not isinstance(paths, list):
//            paths = [paths]
//
//        if not self.has_key(base):
//            self[base] = PBXDict()
//
//        for path in paths:
//            if recursive and not path.endswith('/**'):
//                path = os.path.join(path, '**')
//
//            if not self[base].has_key(key):
//                self[base][key] = PBXList()
//            elif isinstance(self[base][key], basestring):
//                self[base][key] = PBXList(self[base][key])
//
//            if self[base][key].add('\\"%s\\"' % path):
//                modified = True
//
//        return modified
//
//    def add_header_search_paths(self, paths, recursive=True):
//        return self.add_search_paths(paths, 'buildSettings', 'HEADER_SEARCH_PATHS', recursive=recursive)
//
//    def add_library_search_paths(self, paths, recursive=True):
//        return self.add_search_paths(paths, 'buildSettings', 'LIBRARY_SEARCH_PATHS', recursive=recursive)
//
//    def add_other_cflags(self, flags):
//        modified = False
//
//        base = 'buildSettings'
//        key = 'OTHER_CFLAGS'
//
//        if isinstance(flags, basestring):
//            flags = PBXList(flags)
//
//        if not self.has_key(base):
//            self[base] = PBXDict()
//
//        for flag in flags:
//
//            if not self[base].has_key(key):
//                self[base][key] = PBXList()
//            elif isinstance(self[base][key], basestring):
//                self[base][key] = PBXList(self[base][key])
//
//            if self[base][key].add(flag):
//                self[base][key] = [e for e in self[base][key] if e]
//                modified = True
//
//        return modified
	}
}