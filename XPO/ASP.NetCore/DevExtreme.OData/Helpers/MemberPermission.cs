using System.ComponentModel.DataAnnotations;

public class MemberPermission {
    [Key]
    public Guid Key { get; set; }
	public bool Read { get; set; }
	public bool Write { get; set; }

	public MemberPermission() {
		Key = Guid.NewGuid();
	}
}

