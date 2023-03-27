meta:
  id: nut
  file-extension: nut
  title: Namco Texture Bank
  endian: be
seq:
  - id: signature
    type: u4
    enum: nut_signature
    valid:
      any-of:
        - nut_signature::ntp3
        # TODO: - signature::ntwu
        - nut_signature::ntwd
        - nut_signature::ntlx
  - id: body
    type: nut_body

types:
  nothing: { }
  nut_body:
    meta:
      endian:
        switch-on: _root.signature
        cases:
          'nut_signature::ntwd': le
          'nut_signature::ntlx': le
          _: be
    seq:
      - id: header
        type: nut_header
        size: 0xC
      - id: textures
        type: texture(_index)
        repeat: expr
        repeat-expr: header.count
    types:
      nut_header:
        seq:
          - id: version
            type: u2
            valid:
              any-of:
                - 2
          - id: count
            type: u2
      texture:
        params:
          - id: i
            type: s4
        seq:
          - id: size
            type: u4
          - id: padding
            type: nothing
            size: 4
          - id: data_size
            type: u4
          - id: header_size
            type: u2
          - id: texture_data
            # very important this way we save the current header offset
            # and we can calculate the data position properly
            type: texture_data(_root._io.pos.as<s4>)
            size: 2
          - id: texture_info
            type: texture_info
            # total size - header - gidx
            size: header_size - 0x10 - 0x20
          - id: gidx
            type: gidx
            size: 0x20
      texture_data:
        params:
          - id: offset
            type: s4
        instances:
          data_offset:
            value: offset - 0x10 + _parent.texture_info.data_offset
          surfaces:
            io: _root._io
            pos: data_offset
            size: _parent.data_size
            type: texture_surfaces
      gidx:
        seq:
          - id: signature
            contents: [ 'eXt', 0 ]
          - id: version
            type: u4
          - id: version2
            type: u4
          - id: unknown
            type: nothing
            size: 4
          - id: type
            contents: 'GIDX'
          - id: unknown2
            type: nothing
            size: 4
          - id: hash_id
            type: u4
      texture_info:
        seq:
          - id: padding
            type: nothing
            size: 1
          - id: mipmap_count
            type: u1
          - id: padding3
            type: nothing
            size: 1
          - id: pixel_format
            type: u1
            enum: pixel_format
            #valid:
            #  any-of:
            #    - pixel_format::dxt1
            #    - pixel_format::dxt3
            #    - pixel_format::dxt5
          - id: width
            type: u2
          - id: height
            type: u2
          - id: texture_type
            doc: It's a little unclear if this is actually that
            type: u4
            enum: texture_type
            valid:
              any-of:
                - texture_type::dds
          - id: cubemap
            size: 4
            type: cubemap
          - id: data_offset
            type: u4
          - id: padding4
            type: nothing
            size: 12
          - id: cubemap_size_1
            if: cubemap.is_cubemap
            type: u2
          - id: cubemap_size_2
            if: cubemap.is_cubemap
            type: u2
          - id: cubemap_padding
            if: cubemap.is_cubemap
            type: nothing
            size: 8
          - id: mipmap_sizes
            type: u4
            repeat: expr
            repeat-expr: mipmap_count - 1
        instances:
          cubemap_count:
            value: |
              cubemap.sides[0].to_i +
              cubemap.sides[1].to_i +
              cubemap.sides[2].to_i +
              cubemap.sides[3].to_i +
              cubemap.sides[4].to_i +
              cubemap.sides[5].to_i
          surface_count:
            value: "cubemap_count == 0 ? 1 : cubemap_count"
      cubemap:
        seq:
          - id: padding
            type: nothing
            size: 1
          - id: sides
            type: b1
            doc: '+x -x +y -y +z -z'
            repeat: expr
            repeat-expr: 6
          - id: is_cubemap
            type: b1
      texture_surfaces:
        seq:
          - id: surfaces
            type: texture_surface
            repeat: expr
            repeat-expr: _parent._parent.texture_info.surface_count
      texture_surface:
        seq:
          - id: mipmaps
            size: _parent._parent._parent.data_size / _parent._parent._parent.texture_info.surface_count

enums:
  nut_signature:
    0x4E_54_50_33: ntp3
    0x4E_54_57_55: ntwu
    0x4E_54_57_44: ntwd
    0x4E_54_4C_58: ntlx
  pixel_format:
    0x00: dxt1
    0x01: dxt3
    0x02: dxt5
    0x08: rgb16  # TODO...
    0x0C: rgba16 # TODO...
    0x0E: rgba   # TODO...
    0x0F: abgr   # TODO...
    0x10: rgba_2
    0x11: rgba_1
    0x15: rgba_0
    0x16: compressed_rg_rgtc_2
  texture_type:
    0: dds
    1: gxt # TODO
