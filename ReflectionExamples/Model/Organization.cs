﻿namespace ReflectionExamples2.Model {
	public class Organization : Contact {
		private string fileAs;
		public string FileAs {
			get { return this.fileAs; }
			set {
				this.fileAs = value;
			}
		}
	}
}
