meta:
  id: avm1
  title: AVM1 Bytecode
  tags: [ "Flash", "AVM1" ]
  endian: le
  encoding: utf8
seq:
  - id: num_actions
    type: u4
  - id: actions
    type: action
    repeat: expr
    repeat-expr: 5

types:
  action:
    seq:
      - id: size
        type: u2
      - id: padding
        type: nothing
        size: 2
      - id: body
        type: action_body
        size: size - 2
      - id: padding_2
        type: nothing
        size: 2
      - id: align
        type: nothing
        if: (size + 4) % 4 != 0
        size: 4 - (size + 4) % 4
  action_body:
    seq:
      - id: ops
        type: op
        repeat: eos
  op:
    seq:
      - id: offset
        type: instant(_io.pos)
      - id: opcode
        type: u1
        enum: opcode
      - id: action_len
        if: opcode.as<u1> >= 0x80
        type: u2
      - id: action
        if: opcode.as<u1> >= 0x80
        size: action_len.as<u2>
        type:
          switch-on: opcode
          cases:
            'opcode::jump': jump_data
            'opcode::jump_equal': jump_data
            'opcode::push': push_data
            _: nothing
      - id: offset_next
        type: instant(_io.pos)

  instant:
    params:
      - id: value
        type: s8
  nothing: { }
  jump_data:
    seq:
      - id: offset
        type: s2
    instances:
      jump_target_offset:
        value: _parent.offset_next.value + offset
  push_data:
    seq:
      - id: values
        type: push_value
        repeat: eos
  push_value:
    seq:
      - id: type
        type: u1
        enum: value_type
      - id: value
        type:
          switch-on: type
          cases:
            # this diverges from avm1, strings are
            # defined in the string table of the lmd
            # file instead
            'value_type::string': u2
            'value_type::register': u2
            'value_type::int': s4
            'value_type::bool': u1
            'value_type::float': f4
            'value_type::double': f8
            'value_type::constant_pool_8': u1
            'value_type::constant_pool_16': u2
            _: nothing

enums:
  value_type:
    0x00: 'string'
    0x01: 'float'
    0x02: 'null'
    0x03: 'undefined'
    0x04: 'register'
    0x05: 'bool'
    0x06: 'double'
    0x07: 'int'
    0x08: 'constant_pool_8'
    0x09: 'constant_pool_16'
  opcode:
    0x00: end
    0x04: next_frame
    0x05: previous_frame
    0x06: play
    0x07: stop
    0x08: toggle_quality
    0x09: stop_sounds
    0x0A: add
    0x0B: subtract
    0x0C: multiply
    0x0D: divide
    0x0E: equals
    0x0F: less
    0x10: and
    0x11: or
    0x12: not
    0x13: string_equals
    0x14: string_length
    0x15: string_extract
    
    0x17: pop
    0x18: to_integer
    
    0x1C: get_variable
    0x1D: set_variable
    
    0x20: set_target2
    0x21: string_add
    0x22: get_property
    0x23: set_property
    0x24: clone_sprite
    0x25: remove_sprite
    0x26: trace
    0x27: start_drag
    0x28: end_drag
    0x29: string_less
    0x2A: throw
    0x2B: cast_op
    0x2C: implements_op
    
    0x30: random_number
    0x31: mb_string_length
    0x32: char_to_ascii
    0x33: ascii_to_char
    0x34: get_time
    0x35: mb_string_extract
    0x36: mb_char_to_ascii
    0x37: mb_ascii_to_char
    
    0x3A: delete
    0x3B: delete2
    0x3C: define_local
    0x3D: call_function
    0x3E: return_
    0x3F: modulo
    0x40: new_object
    0x41: define_local2
    0x42: init_array
    0x43: init_object
    0x44: type_of
    0x45: target_path
    0x46: enumerate
    0x47: add2
    0x48: less2
    0x49: equals2
    0x4A: to_number
    0x4B: to_string
    0x4C: push_duplicate
    0x4D: stack_swap
    0x4E: get_member
    0x4F: set_member
    0x50: increment
    0x51: decrement
    0x52: call_method
    0x53: new_method
    0x54: instance_of
    0x55: enumerate2
    
    0x60: bit_and
    0x61: bit_or
    0x62: bit_xor
    0x63: bit_l_shift
    0x64: bit_r_shift
    0x65: bit_ur_shift
    0x66: strict_equals
    0x67: greater
    0x68: string_greater
    0x69: extends
    
    0x81: goto_frame
    
    0x83: get_url
    
    0x87: store_register
    0x88: constant_pool
    
    0x8A: wait_for_frame
    0x8B: set_target
    0x8C: goto_label
    0x8D: wait_for_frame_2
    0x8E: define_function_2
    0x8F: try
    
    0x94: with
    
    0x96: push
    
    0x99: jump
    0x9A: get_url_2
    0x9B: define_function
    0x9D: jump_equal
    0x9E: call
    0x9F: goto_frame_2

